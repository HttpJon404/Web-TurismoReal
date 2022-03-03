wt.mensajeria = {
    limpiarTodo: function () {
        Push.clear();
    },
    error: function (mensaje) {
        $(".toast").toast("dispose");
        $('.toast').toast({ delay: 5000 });
        $("#toastTitulo").html("Error");
        $("#toastMensaje").html(mensaje);
        $("#toastImg").attr("src", wt.urlSitio + "Content/Images/atention.png");
        $(".toast").toast("show");
    },

    exito: function (mensaje) {
        $(".toast").toast("dispose");
        $('.toast').toast({ delay: 5000 });
        $("#toastTitulo").html("Éxito");
        $("#toastMensaje").html(mensaje);
        $("#toastImg").attr("src", wt.urlSitio + "Content/Images/check.png");
        $(".toast").toast("show");
    },
    modal_Mensaje_Callback: null,
    modal_Mensaje_Callback_Args: null,
    modal_Mensaje_Tipos: ["info", "question", "warning"],
    modal_Mensaje_Botones: ["si", "no", "cancelar", "aceptar"],

    modal_Mensaje: function (title, message, type, buttons, callback, args) {

        var respuesta = false;
        var ventana = $("#" + "modal-Mensaje");
        // Reiniciar modal
        for (var b in wt.mensajeria.modal_Mensaje_Botones) {
            var boton = $("#" + "modal-Mensaje-Boton-" + wt.mensajeria.modal_Mensaje_Botones[b]);
            if (boton)
                boton.hide();
        }

        //var respuesta = $("#" + "modal-Mensaje-Respuesta");
        var titulo = ventana.find(".modal-title");
        var mensaje = ventana.find("#modal-Mensaje-Texto"); //ventana.find(".modal-body");
        var icono = ventana.find("#modal-Mensaje-Icono");
        if (titulo && title) { titulo.text(title); }
        if (mensaje && message) { mensaje.html(message); }
        if (icono && type) { icono.attr("class", "glyphicon glyphicon-" + type + "-sign icono-mensaje-" + type); }
        if (buttons) {
            for (var b in buttons) {
                var boton = $("#" + "modal-Mensaje-Boton-" + buttons[b]);
                if (boton)
                    boton.show();
            }
        }
        wt.mensajeria.modal_Mensaje_Callback = callback;
        wt.mensajeria.modal_Mensaje_Callback_Args = args;
        ventana.modal();
    },





    modal_Mensaje_Enter: function (title, message, type, buttons, callback, args) {
        var respuesta = false;
        var ventana = $("#" + "modal-Mensaje");
        // Reiniciar modal
        for (var b in wt.mensajeria.modal_Mensaje_Botones) {
            var boton = $("#" + "modal-Mensaje-Boton-" + wt.mensajeria.modal_Mensaje_Botones[b]);
            if (boton)
                boton.hide();
        }
        //var respuesta = $("#" + "modal-Mensaje-Respuesta");
        var titulo = ventana.find(".modal-title");
        var mensaje = ventana.find("#modal-Mensaje-Texto"); //ventana.find(".modal-body");
        var icono = ventana.find("#modal-Mensaje-Icono");
        if (titulo && title) { titulo.text(title); }
        if (mensaje && message) { mensaje.html(message); }
        if (icono && type) { icono.attr("class", "glyphicon glyphicon-" + type + "-sign icono-mensaje-" + type); }
        if (buttons) {
            for (var b in buttons) {
                var boton = $("#" + "modal-Mensaje-Boton-" + buttons[b]);
                if (boton)
                    boton.show();
            }
        }


        wt.mensajeria.modal_Mensaje_Callback = callback;
        wt.mensajeria.modal_Mensaje_Callback_Args = args;
        ventana.modal();
        ventana.on('shown.bs.modal', function () {
            ventana.find(".btn-success").focus();
        });
    },

    modal_Mensaje_Respuesta: function (respuesta) {
        if (wt.mensajeria.modal_Mensaje_Callback != null) {
            var callback = wt.mensajeria.modal_Mensaje_Callback + "('" + wt.mensajeria.modal_Mensaje_Callback_Args.join("','") + "', " + respuesta + ")";
            //alert("callback: " + callback);
            var argument = wt.mensajeria.modal_Mensaje_Callback_Args[0];
            wt.mensajeria.modal_Mensaje_Callback = null;
            wt.mensajeria.modal_Mensaje_Callback_Args = null;
            // Reiniciar modal

            if (respuesta) {
                eval(callback);
            } else {
                this.SeleccionarPuntoInteres(argument);
            }
        }
    }




};