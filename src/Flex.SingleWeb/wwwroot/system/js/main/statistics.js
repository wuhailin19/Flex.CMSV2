var myChart = echarts.init(document.getElementById('maint'));
var option = {
    title: { top: 10, text: '今日访问情况：总浏览量', x: 'center' },
    tooltip: {
        trigger: 'axis',
        axisPointer: {
            type: 'line',
        }
    },
    toolbox: {
        feature: {
            saveAsImage: {}
        }
    },
    legend: {
        top: 30,
        data: ['']
    },
    xAxis:
    {
        data: []
    },
    yAxis: {},
    series: []
};
myChart.setOption(option);