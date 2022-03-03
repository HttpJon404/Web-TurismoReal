wt.pago = {
    arrayAcompanantes: new Array(),
    arrayExtras: new Array(),
    arrayExtrasCreados: new Array(),
    arrayAcompanantesCreados: new Array(),
    redireccionarPago: function (id, rut) {

        
        var fecha1 = $("#fecha_llegada").datepicker('getDate');
        var fecha2 = $("#fecha_salida").datepicker('getDate');

        var stringFecha1 = fecha1.toDateString();
        var stringFecha2 = fecha2.toDateString();

        var valorReserva = $("#valorReserva").val();
        var valorRestante = $("#valorRestante").val();
        //var valorExtras = $("#valorExtras").val();
        var valorTotalDias = $("#valorTotalDias").val();
        var valorDiario = $("#valorDiario").val();

        if (valorReserva === null || valorReserva === '') {
            alertify.alert("Seleccione una fecha de salida e intente nuevamente");
            return;
        }

        if (stringFecha1 === stringFecha2) {
            alertify.alert("Las fechas no deben ser similares");
            return;
        }

        for (var a = 1; a < (wt.pago.arrayAcompanantes.length + 1); a++) {

            var rutAcompanante = $("#txtRutAcompanante-" + a).val();
            var nombreAcompanante = $("#txtNombreAcompanante-" + a).val();
            var apellidoAcompanante = $("#txtApellidoAcompanante-" + a).val();
            var celularAcompanante = $("#txtCelularAcompanante-" + a).val();

            
            var celular = celularAcompanante.toString();
            if (celular.length > 9) {
                alertify.alert("El número del acompañante " + a + " debe ser máximo de 9 digitos");
                return;
            }

            for (var c = 1; c < (wt.pago.arrayAcompanantes.length + 1); c++) {
                for (var d = wt.pago.arrayAcompanantes.length + 1; d--;) {
                    var extra1 = $("#txtRutAcompanante-" + c).val();
                    var extra2 = $("#txtRutAcompanante-" + d).val();

                    if (c !== d) {
                        if (rut === extra1 || rut === extra2) {
                            alertify.alert("No se puede agregar el Rut titular como acompañante");

                            return;
                        }
                        else if (extra1 === extra2) {
                            alertify.alert("No se pueden agregar Rut identicos, re-intente nuevamente");

                            return;
                        }
                    }


                }
            }

            wt.pago.arrayAcompanantesCreados.push({
                rut: rutAcompanante,
                nombre: nombreAcompanante,
                apellido: apellidoAcompanante,
                celular: celular

            });

        }

        var extraTotal = 0;
        for (var b = 1; b < (wt.pago.arrayExtras.length + 1); b++) {

            //var descripcionExtra = $("#txtDescripcionExtra-" + b).val();
            var valorExtra = $("#txtValorExtra-" + b).val();
            var idExtra = $("#cmbIdExtra-" + b).val();
            //var idTransporte = $("txtIdTransporte-" + b).val();
            
            var extra = extraTotal + parseInt(valorExtra);
            extraTotal = extra;
            document.getElementById("valorExtras").value = extraTotal;

            for (var c = 1; c < (wt.pago.arrayExtras.length + 1); c++) {
                for (var d = wt.pago.arrayExtras.length + 1; d--;) {
                    var extra1 = $("#txtValorExtra-" + c).val();
                    var extra2 = $("#txtValorExtra-" + d).val();

                    if (c !== d) {
                        if (extra1 === extra2) {
                            alertify.alert("No se pueden agregar servicios extras identicos, re-intente nuevamente");

                            return;
                        }
                    }
                    

                }
            }



            wt.pago.arrayExtrasCreados.push({
                id: parseInt(idExtra),
                valor: extraTotal

            });

            

        }

        
        

        window.open(wt.urlSitio + "Home/Pago/?id=" + id + "&fecha_llegada=" + stringFecha1 +
            "&fecha_salida=" + stringFecha2 + "&valorReserva=" + valorReserva + "&valorRestante=" +
            valorRestante + "&valorTotalDias=" + valorTotalDias + "&valorDiario=" + valorDiario +
            '&listaAcompanantes=' + JSON.stringify(wt.pago.arrayAcompanantesCreados) + "&listaServicios=" + JSON.stringify(wt.pago.arrayExtrasCreados) + "&valorExtras=" + extraTotal, "_self");



    },
    toDate: function (selector) {
        var from = $(selector).val().split("-");
        return new Date(from[2], from[1] - 1, from[0]);
    },
    confirmarPago: function (idDpto, idUsuario, fechaLlegada, fechaSalida, valorReserva, valorRestante, valorTotalDias, valorDiario, listaAcompanante,listaServicios) {

        var tipoPago = "";
        if ($('#debito').is(':checked')) { tipoPago = "Debito" } else { tipoPago = "Credito"}

        var v_data = new FormData();
        v_data.append('idDpto', idDpto);
        v_data.append('idUsuario', idUsuario);
        v_data.append('fecha_llegada', fechaLlegada);
        v_data.append('fecha_salida', fechaSalida);
        v_data.append('valorReserva', valorReserva);
        v_data.append('valorRestante', valorRestante);
        v_data.append('valorTotalDias', valorTotalDias);
        v_data.append('valorDiario', valorDiario);
        v_data.append('listaAcompanantes', listaAcompanante);
        v_data.append('listaServicios', listaServicios);
        v_data.append('pago', tipoPago);
        

        $.ajax({

            type: 'POST',
            url: wt.urlSitio + 'Home/PagoConfirmado',
            contentType: false, // Not to set any content header  
            processData: false, // Not to process data
            data: v_data

        }).done(function (result) {
            if (result.EsPositiva) {

                alertify.success("Su Reserva fue creada exitosamente, se enviará un correo con su confirmación")
                setTimeout(function () {
                    var url = wt.urlSitio + 'Home/Index';
                    window.location = url;
                    //location.reload();
                },
                    3000);


            }
            else {

                alertify.alert("Ha ocurrido un error en la transacción re-intente nuevamente");
            }
        });

    },
    acompanante: function () {
        var cant = wt.pago.arrayAcompanantes.length;
        if (cant > 6) {
            alertify.error("No se pueden tener más acompañantes");
            return;
        }

        $.ajax({
            type: "POST",
            url: wt.urlSitio + "Home/Acompanante",
            async: false,
            data: {
                id: cant + 1
            }
        }).done(function (result) {

            wt.pago.arrayAcompanantes.push(cant + 1);
            $("#vistaAcompananteRelleno").append(result);
        });

    },
    QuitarUltimoAcompanante: function () {
        if (wt.pago.arrayAcompanantes.length == 1) {
            alertify.error("debe tener al menos un acompañante");
            return;
        }
        var ultimo = wt.pago.arrayAcompanantes.length;
        wt.pago.arrayAcompanantes.pop();
        $("#acompanante-" + ultimo).remove();
    },
    Servicio: function () {
        var cant = wt.pago.arrayExtras.length;
        if (cant > 6) {
            alertify.error("No se pueden tener más servicios extras");
            return;
        }

        $.ajax({
            type: "POST",
            url: wt.urlSitio + "Home/ServiciosExtra",
            async: false,
            data: {
                id: cant + 1
            }
        }).done(function (result) {

            wt.pago.arrayExtras.push(cant + 1);
            $("#vistaServicioRelleno").append(result);
        });

    },
    QuitarUltimoServicio: function () {
        if (wt.pago.arrayExtras.length == 1) {
            alertify.error("debe tener al menos un servicio extra");
            return;
        }
        var ultimo = wt.pago.arrayExtras.length;
        wt.pago.arrayExtras.pop();
        $("#servicio-" + ultimo).remove();
    },
    FormatRut: function (rut) {
        var sRut1 = rut.value;      //contador de para saber cuando insertar el . o la -

        if (sRut1.length < 2) {
            return;
        }

        sRut1 = sRut1.replace("-", "").replace("-", "").replace(".", "").replace(".", "");
        var len = sRut1.length;

        if (len >= 2) {
            sRut1 = sRut1.slice(0, len - 1) + "-" + sRut1.slice(len - 1);
        }
        //Pasamos al campo el valor formateado
        rut.value = sRut1.toUpperCase();
    },
    updateExtra: function (val) {
        var extraTotal = 0;
        for (var c = 1; c < (wt.pago.arrayExtras.length + 1); c++) {
          
            var valorExtra = $("#txtValorExtra-" + c).val();          
            var extra = extraTotal + parseInt(valorExtra);
            extraTotal = extra;                      
        }
        document.getElementById("valorExtras").value = extraTotal;
    }
    ,
    cancelarReserva: function (id) {

        alertify.confirm("¿Desea Cancelar la reserva?",
            function () {
                /*alertify.success('Ok');*/
                var v_data = {
                    id: id
                };
                $.ajax({
                    type: "POST",
                    url: wt.urlSitio + 'Account/CancelarReserva',
                    data: v_data
                }).done(function (result) {
                    if (result.EsPositiva) {
                        alertify.success("Reserva Cancelada con éxito, se enviará un correo con la confirmación");
                        setTimeout(function () {
                            //var url = wt.urlSitio + 'Account/MiPerfilDetalle';
                            //window.location = url;
                            location.reload();
                        },
                            3000);
                    } else {
                        alertify.error(result.Mensaje);
                        
                    }
                    

                });
            },
            function () {
                /*alertify.error('Cancelar');*/
                return;
            });
        
    },
    pagoCheckOut: function (idDetalle) {

        var v_data = {
            id: idDetalle
        };
        $.ajax({
            type: "POST",
            url: wt.urlSitio + 'Home/PagoRestante',
            data: v_data
        }).done(function (result) {
            if (result.EsPositiva) {
                alertify.success("¡Pago Realizado con éxito!");
                setTimeout(function () {
                    var url = wt.urlSitio + 'Account/MiPerfilDetalle';
                    window.location = url;
                    //location.reload();
                },
                    3000);
            } else {
                alertify.error(result.Mensaje);

            }


        });
    },
    redireccionarPagoRestante: function (idDetalle, valorRestante, multa) {

        var detalle = parseInt(idDetalle);
        var restante = parseInt(valorRestante);
        var mul = parseInt(multa);
        window.open(wt.urlSitio + "Home/Pago/?id=" + 0 + "&idDetalle=" + detalle + "&valorRestante=" + restante + "&multa=" + mul, "_self");

    }
};