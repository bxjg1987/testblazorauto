export function init(container, option) {
    let thisECharts = echarts.init(container);
    if (option) {
        thisECharts.setOption(option);
    }
}

export function setOption(container, option) {
    echarts.getInstanceByDom(container).setOption(option);
}

export function dispose(container) {
    echarts.dispose(container);
}