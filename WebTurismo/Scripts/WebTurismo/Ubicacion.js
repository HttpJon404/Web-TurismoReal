wt.ubicacion = {
    Init: function (){


    },
    filtrarUbicacion: function () {

        var region = $("#regiones").val();
        var comuna = $("#comunas").val();
        var direccion = $("#searchDep").val();
        var idRegion = 0;

        if (region === "sin-region") {
            idRegion = 0;
        } 
        else if (region === "Arica y Parinacota") {
            idRegion = 1;
        } else if (region === "Tarapacá") {
            idRegion = 2;
        } else if (region === "Antofagasta") {
            idRegion = 3;
        } else if (region === "Atacama") {
            idRegion = 4;
        } else if (region === "Coquimbo") {
            idRegion = 5;
        } else if (region === "Valparaíso") {
            idRegion = 6;
        } else if (region === "Región Metropolitana de Santiago") {
            idRegion = 7;
        } else if (region === "Región del Libertador Gral. Bernardo O’Higgins") {
            idRegion = 8;
        } else if (region === "Región del Maule") {
            idRegion = 9;
        } else if (region === "Región de Ñuble") {
            idRegion = 10;
        } else if (region === "Región del Biobío") {
            idRegion = 11;
        } else if (region === "Región de la Araucanía") {
            idRegion = 12;
        } else if (region === "Región de Los Ríos") {
            idRegion = 13;
        } else if (region === "Región de Los Lagos") {
            idRegion = 14;
        } else if (region === "Región Aisén del Gral. Carlos Ibáñez del Campo") {
            idRegion = 15;
        } else if (region === "Región de Magallanes y de la Antártica Chilena") {
            idRegion = 16;
        }
        
        //var v_data = {
        //    idRegion: idRegion,
        //    idComuna: comuna,
        //    pagina: 1
        //}
        
        //$.ajax({
        //type: "POST",
        //url: wt.urlSitio + 'Home/Listado',
        //data: v_data
        //});
        window.open(wt.urlSitio + "Home/Listado?idRegion=" + idRegion + "&idComuna=" + comuna + "&direccion=" + direccion + "&pagina=1", '_self');
    }

}