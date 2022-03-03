wt.buscar = {
    Init: function (){

    },
    redireccionarPropiedad: function (id) {
        //window.close();
        window.open(wt.urlSitio + "Home/Propiedad/" + id, '_self');
    },
    redireccionarReserva: function (id) {
        //window.close();
        //if (reserva === false) {
            window.open(wt.urlSitio + "Home/DetalleReserva/" + id, '_self');
        //} else {
        //    alertify.alert("Usted ya posee una reserva actualmente");
        //    return;
        //}
        
    },
    poseeReserva: function () {
        alertify.alert("Usted ya posee una reserva actualmente.");
        return;
    },
    noEsCliente: function () {
        alertify.alert("Usted posee un rol distinto a Cliente");
        return;
    }
    
}