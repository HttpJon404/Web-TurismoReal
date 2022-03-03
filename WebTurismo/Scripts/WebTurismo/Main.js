wt = {
    urlSitio: null,
    Init: function () {

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
    stringToDate: function (_date, _format, _delimiter) {
        var hours = _date.split(' ')[1];
        var hour = hours.split(':')[0];
        var min = hours.split(':')[1];
        _date = _date.split(' ')[0];
        var formatLowerCase = _format.toLowerCase();
        var formatItems = formatLowerCase.split(_delimiter);
        var dateItems = _date.split(_delimiter);
        var monthIndex = formatItems.indexOf("mm");
        var dayIndex = formatItems.indexOf("dd");
        var yearIndex = formatItems.indexOf("yyyy");
        var month = parseInt(dateItems[monthIndex]);
        month -= 1;
        var formatedDate = new Date(dateItems[yearIndex], month, dateItems[dayIndex], hour, min);
        return formatedDate;
    },
    ExCorreo: function (correo) {
        return (/^(([^<>()[\]\.,;:\s@\"]+(\.[^<>()[\]\.,;:\s@\"]+)*)|(\".+\"))@(([^<>()[\]\.,;:\s@\"]+\.)+[^<>()[\]\.,;:\s@\"]{2,})$/i.test(correo))
    }

}