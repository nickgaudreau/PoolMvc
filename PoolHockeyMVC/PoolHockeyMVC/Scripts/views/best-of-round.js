$(document).ready(function () {

    /********Partial Ajax********/
    $(document).ajaxStart(function () {
        $('#loading').show();
    });

    $(document).ajaxStop(function () {
        $('#loading').hide();
    });



    $('.pagination > button').click(function () {
        var round = $(this).text();

        $.ajax({
            type: 'GET',
            url: '@Url.Action("SetBestOfRound", "UserInfo")',
            traditional: true, // this is needed for MVC ajax calling process
            data:
            {
                round: round
            },
            success: successFunc,
            error: errorFunc
        });
    });

    function successFunc(data, status) {
        $('#data').html(data);
    }

    function errorFunc(jqXHR, exception) {
        var msg = '';
        if (jqXHR.status === 0) {
            msg = 'Not connect.\n Verify Network.';
        } else if (jqXHR.status == 404) {
            msg = 'Requested page not found. [404]';
        } else if (jqXHR.status == 500) {
            msg = 'Internal Server Error [500].';
        } else if (exception === 'parsererror') {
            msg = 'Requested JSON parse failed.';
        } else if (exception === 'timeout') {
            msg = 'Time out error.';
        } else if (exception === 'abort') {
            msg = 'Ajax request aborted.';
        } else {
            msg = 'Uncaught Error.\n' + jqXHR.responseText;
        }
        alert("Error... details: " + msg);
    }



});
