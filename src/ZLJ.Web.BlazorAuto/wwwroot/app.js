
//还是通过refresh_token方式实现吧
////accessToken
//var accessToken = '';
////最后获取accessToken的时间点
//var lastGetAccessTokenTime = new Date();

//setInterval(function () {


//    // 使用 axios.get 方法
//    axios.get('http://example.com/api/data')
//        .then(function (response) {
//            console.log(response.data);
//        })
//        .catch(function (error) {
//            console.log(error);
//        });


//}, 1);


function hasClass(element, className) {
    // 检查元素的 className 是否包含指定的类名  
    return (' ' + element.className + ' ').indexOf(' ' + className + ' ') > -1;
}

function addClass(element, className) {
    // 如果元素没有指定的类名，则添加它  
    console.log('添加样式');
    console.log(element.className);
    if (!hasClass(element, className)) {
        element.className += ' ' + className;
        console.log(element.className);
    }
}

function removeClass(element, className) {
    // 如果元素有指定的类名，则移除它  
    if (hasClass(element, className)) {
        element.className = element.className.replace(new RegExp('(^|\\b)' + className.split(' ').join('|') + '(\\b|$)', 'gi'), ' ');
    }
}
//如果元素已经拥有这个 class，那么就会移除它；如果元素没有这个 class，那么就会添加它。
function toggleClass(element, className) {
    if ('classList' in element) {
        console.log('浏览器支持classList');
        // 如果浏览器支持 classList，使用 toggle 方法  
        element.classList.toggle(className);
    } else {
        console.log('浏览器不支持classList');
        //如果浏览器不支持 classList，使用 hasClass、addClass 和 removeClass  
        if (hasClass(element, className)) {
            removeClass(element, className);
        } else {
            addClass(element, className);
        }
    }
}


//
function refreshCaptcha(img) {
    var sjz = generateUniqueID();
   
    var url = serverRootAddress + "/api/Captcha/Captcha?id=" + sjz;
    img.src = url;
    //console.log(url);
    return sjz;
};



function generateUniqueID() {
    // 获取当前时间戳（毫秒）
    const timestamp = Date.now();

    // 生成加密安全的随机字节数组（12字节 = 96位）
    const randomBytes = new Uint8Array(12);
    if (window.crypto && window.crypto.getRandomValues) {
        // 浏览器环境，使用加密安全的随机数生成
        window.crypto.getRandomValues(randomBytes);
    } else {
        // 回退到Math.random()（安全性较低，适用于旧环境）
        for (let i = 0; i < 12; i++) {
            randomBytes[i] = Math.floor(Math.random() * 256);
        }
    }

    // 将字节数组转换为十六进制字符串（24位）
    const randomPart = Array.from(randomBytes)
        .map(b => b.toString(16).padStart(2, '0'))
        .join('');

    // 返回组合后的唯一ID
    return `${timestamp}${randomPart}`;
}