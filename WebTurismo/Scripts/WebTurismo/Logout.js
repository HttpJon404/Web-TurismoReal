wt.logout = {
    Init: function () {

    },
    LogoutUsuario: function () {
        $.ajax({
            type: "POST",
            url: wt.urlSitio + 'Account/LogoutUsuario',
            data: null
        }).done(function (result) {
            if (result.EsPositiva) {

                
                
                alertify.success("¡Hasta pronto :D!")
                setTimeout(function () {
                    var url = wt.urlSitio + 'Home/Index';
                    window.location = url;
                    location.reload();
                },
                    3000);

            
            }
            else {
                alertify.error(result.Mensaje);
            }
        });


    }
}
