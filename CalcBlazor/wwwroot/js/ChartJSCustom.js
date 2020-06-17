window.chartJSCustom = {
    revrseChartY: function (id) {
        window.console.log("geting chart object");
        var ctx = document.getElementById(id).getContext('2d');
        const chart = new Chart(ctx);

        window.console.log(chart);
        chart.config.options.title = "BLABLA";
        chart.config.options.responsive = true;
        chart.config.options.scales = {
            xAxes: [{
                type: 'linear',
                display: true,
                ticks: {

                    beginAtZero: false,
                }, scaleLabel: {
                    display: true,
                    labelString: 'MRd'
                },
                position: 'bottom'
            }],
            yAxes: [{
                type: 'linear',
                display: true,
                ticks: {
                    reverse: true,
                    beginAtZero: false,
                }, scaleLabel: {
                    display: true,
                    labelString: 'NRd'
                }
            }]
        };
        //chart.config.options = {
        //    responsive: true,
        //    scales: {
        //        xAxes: [{
        //            type: 'linear',
        //            display: true,
        //            ticks: {

        //                beginAtZero: false,
        //            }, scaleLabel: {
        //                display: true,
        //                labelString: 'MRd'
        //            },
        //            position: 'bottom'
        //        }],
        //        yAxes: [{
        //            type: 'linear',
        //            display: true,
        //            ticks: {
        //                reverse: true,
        //                beginAtZero: false,
        //            }, scaleLabel: {
        //                display: true,
        //                labelString: 'NRd'
        //            }
        //        }]
        //    }
        //};

        chart.update();
    }
};