
window.chartJsInterop = {
    initializeChart: function (canvasId, data) {
        var ctx = document.getElementById(canvasId).getContext('2d');
        new Chart(ctx, {
            type: 'bar',
            data: {
                labels: ['January', 'February', 'March', 'April', 'May', 'June', 'July', 'August', 'September', 'October', 'November', 'December'],
                datasets: [{
                    label: 'Monthly Expenses ($)',
                    data: data,
                    // data: [1500, 1800, 2200, 1300, 2500, 2400, 2700, 2900, 2000, 1600, 1900, 2100],
                    backgroundColor: 'rgba(75, 192, 192, 0.2)',
                    borderColor: 'rgba(75, 192, 192, 1)',
                    borderWidth: 1
                }]
            },
            options: {
                scales: {
                    y: {
                        beginAtZero: true,
                        max: 5000,
                        ticks: {
                            stepSize: 200
                        },
                        title: {
                            display: true,
                            text: 'Amount ($)'
                        }
                    },
                    x: {
                        title: {
                            display: true,
                            text: 'Months'
                        }
                    }
                }
            }
        });
    }
};


function showHidePasswordField(fieldId)
{
    $(`#${fieldId} a`).on('click', function(event) {
        event.preventDefault();
        console.log(fieldId)
        if($(`#${fieldId} input`).attr("type") === "text")
        {
            $(`#${fieldId} input`).attr('type', 'password');
            $(`#${fieldId} i`).addClass( "fa-eye-slash" );
            $(`#${fieldId} i`).removeClass( "fa-eye" );
        }
        else if($(`#${fieldId} input`).attr("type") === "password")
        {
            $(`#${fieldId} input`).attr('type', 'text');
            $(`#${fieldId} i`).removeClass( "fa-eye-slash" );
            $(`#${fieldId} i`).addClass( "fa-eye" );
        }
    });

}