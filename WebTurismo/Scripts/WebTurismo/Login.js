wt.login = {
    Init: function () {

    },
    AbrirLogin: function () {
        $('a[href="#registrar"]').removeClass("active");
        $('a[href="#iniciosesion"]').addClass("active");
        $("#registrar").removeClass("active");
        $("#iniciosesion").addClass("active");
        $("#iniciosesion").addClass("show");

        $("#ModalRegistroLogin").modal('show');
    },
    LoginUsuario: function () {
        var correo = $("#idCorreo").val();
        correo = correo.toLowerCase();
        var contrasena = $("#idContrasena").val();
        var v_data = {
            // tiene que tener los mismos nombres de las variables que pide el método del controlador
            correo: correo,
            contrasena: contrasena
        };

        $.ajax({
            type: "POST",
            url: wt.urlSitio + 'Home/LoginUsuario',
            data: v_data

        }).done(function (result) {
            if (result.EsPositiva) {
                alertify.success('Has iniciado sesión correctamente');
                setTimeout(function () {
                    location.reload();
                }, 3000);
                
                
            }
            else {

                setTimeout(function () {
                    alertify.error(result.Mensaje);
                },
                    3000);

            }

        });
        
    },
    DetalleReserva: function (id) {
        var v_data = {
            idDetalle: id
        };
        $.ajax({
            type: "POST",
            url: wt.urlSitio + 'Account/GenerarDetalle',
            data: v_data
        }).done(function (result) {
            window.open( wt.urlSitio + 'Imagen/PreviewPdf?file=' + result +'&contentType=application/pdf',"_blank");
            

        });
    },
    cambiarContraseñaPerfil: function (id) {
        var contrasena1 = $("#contraseña1").val();
        var contrasena2 = $("#contraseña2").val();
        var idUsu = id.toString();

        var v_data = {
            id: idUsu,
            pass: contrasena1,
            pass2: contrasena2
        };

        $.ajax({
            type: "POST",
            url: wt.urlSitio + 'Home/Restablecer',
            data: v_data
        }).done(function (result) {
            alertify.success("Contraseña Actualizada Correctamente.");


        });
    }
}