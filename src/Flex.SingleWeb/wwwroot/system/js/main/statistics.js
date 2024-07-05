var myChart = echarts.init(document.getElementById('maint'));
var option = {
    title: { top: 10, text: '今日访问情况：UV（32）PV（122）', x: 'center' },
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
        data: ['00:00~01:00', '01:00~02:00', '02:00~03:00', '03:00~04:00', '04:00~05:00', '05:00~06:00', '06:00~07:00', '07:00~08:00', '08:00~09:00', '09:00~10:00', '10:00~11:00', '11:00~12:00', '12:00~13:00', '13:00~14:00', '14:00~15:00', '15:00~16:00', '16:00~17:00', '17:00~18:00', '18:00~19:00', '19:00~20:00', '20:00~21:00', '21:00~22:00', '22:00~23:00', '23:00~23:59',]
    },
    yAxis: {},
    series: [
        { name: 'uv', type: 'line', smooth: true, itemStyle: { normal: { areaStyle: { type: 'default' } } }, data: [1, 2, 1, 4, 2, 6, 3, 2, 4, 1, 1, 1, 1, 2, 3, 2, 0, 0, 0, 0, 0, 0, 0, 0,] },
        { name: 'pv', type: 'line', smooth: true, itemStyle: { normal: { areaStyle: { type: 'default' } } }, data: [1, 2, 43, 4, 22, 6, 37, 8, 9, 1, 2, 3, 4, 5, 6, 6, 0, 0, 0, 0, 0, 0, 0, 0,] },]
};
myChart.setOption(option);