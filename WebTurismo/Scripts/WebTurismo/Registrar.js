wt.registrar = {
    Init: function () {

    },
    RegistrarUsuario: function () {
        var contrasena1 = $("#contrasena").val();
        var contrasena2 = $("#contrasena2").val();
        if (contrasena1 != contrasena2) {
            alertify.alert("Las contraseñas deben ser identicas");
            return;
        }
        var celular = $("#celular").val();
        if (celular < 100000000 || celular > 999999999) {
            setTimeout(function () {
                alertify.alert("El numero de teléfono debe tener 9 digitos");
                
            },
                1000);
            return;
        }
        var correo = $("#correo").val();
        if (!wt.ExCorreo(correo)) {
            setTimeout(function () {
                alertify.alert("El correo no es valido");
                
            },
                1000);
            return;
        }
        var edad = $("#edad").val();
        if (edad > 100 || edad<18) {
            setTimeout(function () {
                alertify.alert("Ingrese una edad correcta, debe ser mayor de edad");

            },
                1000);
            return;
        }
        correo = correo.toLowerCase();
        var v_data = {

            
            nombre: $("#nombres").val(),
            apellidos: $("#apellidos").val(),
            edad: edad,
            RUT: $("#rut").val(),
            idGenero: $("#idGenero").val(),
            idComuna: $("#idComuna").val(),
            idRegion: $("#idRegion").val(),
            direccion: $("#Direccion").val(),
            correo: correo,          
            celular: celular,
            Contrasena: $("#contrasena").val(),
            idRol: 2

        };

        $.ajax({

            type: "POST",
            url: wt.urlSitio + 'Home/RegistrarUsuario',
            data: v_data

        }).done(function (result) {
            if (result.EsPositiva) {
                alertify.success(result.Mensaje)
                setTimeout(function () {
                   
                    location.reload();
                },
                    2000);
                
                
            }
            else {

                alertify.error(result.Mensaje);
            }
        });
    }

}


