(function () {

    //所有页面共享的js功能（包含登录页、首页和其它...）

    abp.appPath = 'http://localhost:21021/';

    /*  来自文档
        Default Localization Source
        You can set a default localization source and use the abp.localization.localize method without the source name.

        Copy
        abp.localization.defaultSourceName = 'SimpleTaskSystem';
        var s1 = abp.localization.localize('NewTask');
        defaultSourceName is global and works for only one source at a time.
    */
    abp.localization.defaultSourceName = 'ZLJ';    //abp模板中没有提供这个值，设置默认本地化资源
    abp.localization.zlj = abp.localization.getSource(abp.localization.defaultSourceName);//后来发现没啥必要，可以这样：abp.localization.localize('NewTask');
    abp.localization.bxjgUtils = abp.localization.getSource('BXJGUtils');
    abp.localization.bxjgShop = abp.localization.getSource('BXJGShop');
    abp.localization.bxjgCMS = abp.localization.getSource('BXJGCMS');
    //除了abp的ajax请求，其它所有ajax请求异常时的处理，比如：easyui列表页请求异常
    abp.ajax.myErrorHandler = function (jqXHR, status, error) {
        //|| (jqXHR.status === 500 && jqXHR.data.error)
        if (jqXHR.status === 401) {
            abp.ajax.handleUnAuthorizedRequest(
                abp.ajax.showError(abp.ajax.defaultError401),
                'login.html'
            );
            //abp.ajax.showError(abp.ajax.defaultError401)
            //.done(function () {
            //    location = 'login.html';
            //});
            return;
        }
        if (jqXHR.status === 403) {
            abp.ajax.showError(abp.ajax.defaultError403);
            return;
        }
        if (jqXHR.status === 404) {
            abp.ajax.showError(abp.ajax.defaultError404);
            return;
        }
        try {
            abp.ajax.showError(jqXHR.responseJSON.error);
            //abp.message.error(jqXHR.responseJSON.error.details,jqXHR.responseJSON.error.message);
        } catch (e) {
            abp.ajax.showError(abp.ajax.defaultError);
        }
    };

    $.ajaxSetup({
    //    //经过测试abp.ajax.ajaxSendHandler的设置对这里有效
    //    //abp.ajax的请求测试过,不会调用这里
    //    //beforeSend: function (xhr) {
    //    //    let tk = abp.getCurrentJWTToken();
    //    //    if (tk) {
    //    //        console.log('原始ajax请求添加accessToken');
    //    //        xhr.setRequestHeader("Authorization", "Bearer " + tk);
    //    //    }
    //    //},

    //    //普通ajax异常，经过测试abp.ajax会引用这里，但easyui直接请求异常不会引用这里
        error: abp.ajax.myErrorHandler
    });

    //abp.jquery.js中$(document).ajaxSend(function ，直接原始ajax请求前会调用这个，所以上面的$.ajaxSetup可以不用了
    //abp.ajax请求前添加accessToken，它内部没有使用jq.ajax的默认参数
    //经过测试在$.ajaxSetup中添加accessToken不行，但是在这里加的accessToken会影响原始$.ajax
    var oldAjaxSendHandler = abp.ajax.ajaxSendHandler;
    abp.ajax.ajaxSendHandler = function (event, request, settings) {
        if (oldAjaxSendHandler)
            oldAjaxSendHandler(event, request, settings);

        let tk = abp.getCurrentJWTToken();
        if (tk) {
            //console.log('abp.ajax请求添加accessToken');
            request.setRequestHeader("Authorization", "Bearer " + tk);
        } 
    };
    
    //处理401 abp.ajax请求后若是未授权则处理登陆重新获取accessToken
    var oldHandleUnAuthorizedRequest = abp.ajax.handleUnAuthorizedRequest;
    abp.ajax.handleUnAuthorizedRequest = function (defMsg) {
        //console.log('没授权的处理：' + defMsg);
        oldHandleUnAuthorizedRequest(defMsg, 'login.html');
    };

    //不要加，因为一旦加上就无法取消
    //$.serializeJSON.defaultOptions.checkboxUncheckedValue = 'false';

    //当前语言------------------------------------------------------start
    abp.currentLanguageKey = 'currentLanguage';
    abp.getCurrentLanguage = function () {
        if (window.localStorage)
            return window.localStorage.getItem(abp.currentLanguageKey);
        else
            return abp.utils.getCookieValue(abp.currentLanguageKey);
    };
    abp.setCurrentLanguage = function (val) {
        if (window.localStorage)
            return window.localStorage.setItem(abp.currentLanguageKey,val);
        else
            return abp.utils.setCookieValue(abp.currentLanguageKey, val);
    };
    //当前语言--end

    //当前jwtToken--------------------------------------------------start
    abp.currentJWTTokenKey = 'currentJWTToken';
    abp.getCurrentJWTToken = function () {
        if (window.localStorage)
            return window.localStorage.getItem(abp.currentJWTTokenKey);
        else
            return abp.utils.getCookieValue(abp.currentJWTTokenKey);
    };
    abp.setCurrentJWTToken = function (val) {
        if (window.localStorage)
            return window.localStorage.setItem(abp.currentJWTTokenKey,val);
        else
            return abp.utils.setCookieValue(abp.currentJWTTokenKey,val);
    };
    //当前jwtToken--------------------------------------------------end
})();
