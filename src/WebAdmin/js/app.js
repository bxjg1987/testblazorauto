//此文件必须在abp.js abp.jquery.js之后，在其它业务逻辑js之前加载，在startup.js前后加载都行
//后台首页和其它业务逻辑页面共用
(function () {

    abp.validation = abp.validation || {};
    abp.validation.checkPhone = function (z_check_value) {
        //if (isEmpty(z_check_value) || z_check_value.length !== 11)
        //    return false;
        var reg = /^1[0-9]{10}$/;
        return reg.test(z_check_value);
    };
    abp.validation.checkIdCard = function (id) {
        // 1 "验证通过!", 0 //校验不通过 // id为身份证号码
        var format = /^(([1][1-5])|([2][1-3])|([3][1-7])|([4][1-6])|([5][0-4])|([6][1-5])|([7][1])|([8][1-2]))\d{4}(([1][9]\d{2})|([2]\d{3}))(([0][1-9])|([1][0-2]))(([0][1-9])|([1-2][0-9])|([3][0-1]))\d{3}[0-9xX]$/;
        //号码规则校验
        if (!format.test(id)) {
            return { 'status': 0, 'msg': '身份证号码不合规' };
        }
        //区位码校验
        //出生年月日校验  前正则限制起始年份为1900;
        var year = id.substr(6, 4),//身份证年
            month = id.substr(10, 2),//身份证月
            date = id.substr(12, 2),//身份证日
            time = Date.parse(month + '-' + date + '-' + year),//身份证日期时间戳date
            now_time = Date.parse(new Date()),//当前时间戳
            dates = (new Date(year, month, 0)).getDate();//身份证当月天数
        if (time > now_time || date > dates) {
            return { 'status': 0, 'msg': '出生日期不合规' }
        }
        //校验码判断
        var c = new Array(7, 9, 10, 5, 8, 4, 2, 1, 6, 3, 7, 9, 10, 5, 8, 4, 2);  //系数
        var b = new Array('1', '0', 'X', '9', '8', '7', '6', '5', '4', '3', '2'); //校验码对照表
        var id_array = id.split("");
        var sum = 0;
        for (var k = 0; k < 17; k++) {
            sum += parseInt(id_array[k]) * parseInt(c[k]);
        }
        if (id_array[17].toUpperCase() !== b[sum % 11].toUpperCase()) {
            return { 'status': 0, 'msg': '身份证校验码不合规' };
        }
        return { 'status': 1, 'msg': '校验通过' };
    };

    //扩展easyui
    abp.easyui = abp.easyui || {};

    abp.easyui.serializejsonSwitchbutton = function (selector) {
        //除了switchbutton，也可以处理其它控件

        if (!(selector instanceof jQuery))
            selector = $(selector);

        setTimeout(function () {
            let sw = selector.find('.easyui-switchbutton');
            if (!sw)
                return;

            $.each(sw, function (index, currentValue) {
                currentValue = $(currentValue);
                var ys = currentValue.next().find('input');
                if (currentValue.attr('data-value-type'))
                    ys.attr('data-value-type', currentValue.attr('data-value-type'));
                if (currentValue.attr('value'))
                    ys.attr('value', currentValue.attr('value'));
                if (currentValue.attr('data-unchecked-value'))
                    ys.attr('data-unchecked-value', currentValue.attr('data-unchecked-value'));
            });
        }, 0);
    };

    var inputWidth = 170;
    if ($.fn.textbox) {
        $.fn.textbox.defaults.width = inputWidth;
        //$.fn.textbox.defaults.validateOnCreate = false;
        //$.fn.textbox.defaults.validateOnBlur = true;
    }
    if ($.fn.datebox) {
        $.fn.datebox.defaults.width = inputWidth;
        //$.fn.datebox.defaults.validateOnCreate = false;
        //$.fn.datebox.defaults.validateOnBlur = true;
    }
    if ($.fn.datetimebox) {
        $.fn.datetimebox.defaults.width = inputWidth;
        //$.fn.datetimebox.defaults.validateOnCreate = false;
        //$.fn.datetimebox.defaults.validateOnBlur = true;
    }
    if ($.fn.combobox) {
        //$.fn.combobox.defaults.validateOnCreate = false;
        //$.fn.combobox.defaults.validateOnBlur = true;
        $.fn.combobox.defaults.width = inputWidth;
        $.fn.combobox.defaults.prompt = '==请选择==';
        $.fn.combobox.defaults.panelHeight = 'auto';
        $.fn.combobox.defaults.panelMaxHeight = '280';
        $.fn.combobox.defaults.onLoadError = abp.ajax.myErrorHandler;
        //$.fn.combobox.defaults.textField = 'displayText';
    }
    if ($.fn.combogrid) {
        $.fn.combogrid.defaults.width = inputWidth;
        $.fn.combogrid.defaults.onLoadError = abp.ajax.myErrorHandler;
    }
    if ($.fn.numberspinner) {
        $.fn.numberspinner.defaults.width = inputWidth;
    }
    if ($.fn.combotree) {
        $.fn.combotree.defaults.width = inputWidth;
        //$.fn.combotree.defaults.validateOnCreate = false;
        //$.fn.combotree.defaults.validateOnBlur = false;
        $.fn.combotree.defaults.prompt = '==请选择==';
        $.fn.combotree.defaults.panelHeight = 'auto';
        $.fn.combotree.defaults.panelMaxHeight = '280';
        $.fn.combotree.defaults.onLoadError = abp.ajax.myErrorHandler;
        $.fn.combotree.defaults.onChange = function (n, o) {
            //console.log(n);
            if (n === 'null')
                $(this).combotree('clear');
        };
    }
    if ($.fn.tree) {
        $.fn.tree.defaults.onLoadError = abp.ajax.myErrorHandler;
        $.fn.tree.defaults.method = 'get';
        $.fn.tree.defaults.loadFilter = function (data, parent) {
            if (data && data.__abp) {
                return  data.result.items;
            }
            return data;
        };
    }
    if ($.fn.validatebox) {
        $.fn.validatebox.defaults.width = inputWidth;

        //easyui验证器扩展
        $.extend($.fn.validatebox.defaults.rules, {
            phone: {
                //手机号码校验
                validator: function (value, param) {
                    return abp.validation.checkPhone(value);
                },
                message: '手机号码格式错误！'
            },
            idcard: {
                //手机号码校验
                validator: function (value, param) {
                    return abp.validation.checkIdCard(value).status === 1;
                },
                message: '身份证号格式错误！'
            },
            passwordEquals: {
                validator: function (value, param) {
                    return value === $(param[0]).val();
                },
                message: '密码确认无效！'
            }
        });
    }
    if ($.fn.switchbutton) {
        $.fn.switchbutton.defaults.onText = '是';
        $.fn.switchbutton.defaults.offText = '否';
    }
    if ($.fn.checkbox) {
        $.fn.checkbox.defaults.labelPosition = 'after';
    }
    if ($.fn.datagrid) {
        $.fn.datagrid.defaults.onBeforeLoad = function (param) {
            if (param && param.hasOwnProperty('page')) {
                param.skipCount = (param.page - 1) * param.rows;
                param.maxResultCount = param.rows;
                delete param.rows;
                delete param.page;
            }
            if (param && param.hasOwnProperty('sort')) {
                param.sorting = param.sort + ' ' + param.order;
                delete param.sort;
                delete param.order;
            }
        };
        $.fn.datagrid.defaults.loadFilter = function (data) {
            if (data && data.__abp)
                return { rows: data.result.items, total: data.result.totalCount };
            return data;
        };
        $.fn.datagrid.defaults.onLoadError = abp.ajax.myErrorHandler;
        $.fn.datagrid.defaults.method = 'get';
        //$.fn.datagrid.defaults.ctrlSelect = true;
        //$.fn.datagrid.defaults.fit = true;
        //$.fn.datagrid.defaults.checkOnSelect = true;
        //$.fn.datagrid.defaults.selectOnCheck = true;
        //$.fn.datagrid.defaults.singleSelect = true;
        //$.fn.datagrid.defaults.pageList = [15, 30, 50, 80, 100, 150, 200];
        //$.fn.datagrid.defaults.pageSize = 15;
        //$.fn.datagrid.defaults.border = false;
        //$.fn.datagrid.defaults.rownumbers = true;
        //$.fn.datagrid.defaults.pagination = true;
    }
    if ($.fn.treegrid) {
        $.fn.treegrid.defaults.onBeforeLoad = function (param) {
            //没有处理abp api 返回保证json的情况
            if (param && param.page) {
                param.skipCount = (param.page - 1) * param.rows;
                param.maxResultCount = param.rows;
                delete param.rows;
                delete param.page;
            }
            if (param && param.sort) {
                param.sorting = param.sort + ' ' + param.order;
                delete param.sort;
                delete param.order;
            }
        };
        $.fn.treegrid.defaults.loadFilter = function (data) {
            if (data && data.totalCount)
                return { rows: data.items, total: data.totalCount };
            return data;
        };
        $.fn.treegrid.defaults.onLoadError = abp.ajax.myErrorHandler;
        //$.fn.datagrid.defaults.ctrlSelect = true;
        //$.fn.datagrid.defaults.fit = true;
        //$.fn.datagrid.defaults.checkOnSelect = true;
        //$.fn.datagrid.defaults.selectOnCheck = true;
        //$.fn.datagrid.defaults.singleSelect = true;
        //$.fn.datagrid.defaults.pageList = [15, 30, 50, 80, 100, 150, 200];
        //$.fn.datagrid.defaults.pageSize = 15;
        //$.fn.datagrid.defaults.border = false;
        //$.fn.datagrid.defaults.rownumbers = true;
        //$.fn.datagrid.defaults.pagination = true;
    }


    //常用扩展
    String.prototype.startWith = function (str) {
        var reg = new RegExp("^" + str);
        return reg.test(this);
    };
    String.prototype.endWith = function (str) {
        var reg = new RegExp(str + "$");
        return reg.test(this);
    };



    //文件管理相关的公共方法
    abp.file = abp.file || {};

    //生成上传附件的选项
    abp.file.buildJQUOA = function (module, permission,downPermission) {
        return {
            server: abp.appPath + '/api/attachment/upload',
            onCheckUpload: function (ct) {
                return abp.services.bxjg.bXJGAttachment.checkUpload({
                    "module": module,
                    "permission": permission,
                    "size": ct.file.size / 1024,
                    "extension": '.' + ct.file.ext,
                    //"fileOperation": 0,
                    "mD5": ct.fileMd5
                });
            },
            formData: { module: module, permission: permission },
            columnButtons: [{
                checkVisiable: function (ct) {
                    return ct.attachmentId;
                },
                size1: 'large',
                text1: '下载',
                iconCls: 'icon-download',
                plain: true,
                onClick: function (ct) {
                    abp.ajax({
                        url: abp.appPath + 'api/attachment/downloadstart',
                        type:'post',
                        data: JSON.stringify({ id: ct.attachmentId, permission: downPermission }),
                        success: function (data) {
                            window.open(abp.appPath + 'api/attachment/downloadend?token='+data);
                        }
                    });
                }
            }]
        };
    };

    //文件上传前设置accessToken
    if ($.fn.jqEasyuiWebuploader) {
        $.fn.jqEasyuiWebuploader.defaults.onChunkBeforeUpload = function (ct) {
            ct.headers.Authorization = "Bearer " + abp.auth.accessToken;
        };
        $.fn.jqEasyuiWebuploader.defaults.swf = '/lib/webuploader-0.1.5/Uploader.swf';
    }
})();