// relies on jQuery.


function login() {
    var username = $('#username').val();
    var password = $('#password').val();

    var model = {
        userName: username,
        password: password
    };


    $.ajax({
        type: "POST",
        url: "/api/auth/login",
        data: JSON.stringify(model),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            if (data.token) {
                localStorage.setItem("jwt", data.token);
                $("#response").text("Logged In");

                // Could set the auth for all future ajax calls like this, but I've done it individually below.
                //var authString = 'Bearer ' + data.token;
                //$.ajaxSetup({
                //    beforeSend: function (xhr) {
                //        xhr.setRequestHeader('Authorization', authString);
                //    }
                //});


            } else {
                $("#response").text("Logged Failed. Debug for info");  
            }
            console.log('Login Result: ' + data);
        },
        failure: function (errMsg) {
            alert(errMsg);
        }
    });

}



function callProtectedApi() {

    var jwt = localStorage.getItem("jwt");

    var authString = 'Bearer ' + jwt; // Authorization: 'Bearer o849ujqpfvnq490ru...'

    $.ajax({
        type: "GET", //GET, POST, PUT
        url: '/api/protected/get?inputMessage=test',  //the url to call (has a parameter on the call)
        contentType: "application/json; charset=utf-8",
        beforeSend: function (xhr) {
            xhr.setRequestHeader('Authorization', authString);
        }
    }).done(function (response) {
        $("#response").text(response.message);
    }).fail(function (err) {
        //Error during request
        var y = err;
        console.log(err);
        $("#response").text('Error - see console.');
    });


}