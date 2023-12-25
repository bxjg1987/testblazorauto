
export function init(container, option) {
    let thisECharts = echarts.init(container);
    if (option) {
        thisECharts.setOption(option);
    }

    //try {
        container.echarts = thisECharts;
    //} catch (e) {
    //    console.log('给容器赋值失败！', e);
    //}

    //window.onresize = function () {
    //    thisECharts.resize();
    //};

    container.resizeHandler = function () {
        thisECharts.resize();
    };

    window.addEventListener('resize', container.resizeHandler);
}

export function setOption(container, option) {
    if (!echarts) {
        container.innerText = 'echarts尚未初始化完成，请稍后刷新重试。';
        return;
    }
    echarts.getInstanceByDom(container).setOption(option);
    //console.log('带回来了', container.echarts);
}

export function dispose(container) {
    //console.log('组件被释放了', container.echarts, container.resizeHandler);
    window.removeEventListener('resize', container.resizeHandler);
    echarts.dispose(container);
}