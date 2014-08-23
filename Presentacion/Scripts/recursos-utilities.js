var tiposDeRecursosJSON;
var tiposDeCaracteristicas;

(function ($) {
    $.fn.toggleDisabled = function () {
        return this.each(function () {
            this.disabled = !this.disabled;
        });
    };
})(jQuery);

function replaceAll(find, replace, str) {
    return str.replace(new RegExp(find, 'g'), replace);
}

function CambiarTiposDeRecursos() {
    // Vaciar el contenedor de caracteristicas
    $('#characteristics-container').empty();

    // Obtener los tipos de caracteristicas para el tipo de recurso actual
    var tiposDeCaracteristicas;

    $.each(tiposDeRecursosJSON, function (i, value) {
        if (value['Id'] == $('#TipoId').val()) {
            tiposDeCaracteristicas = value['TiposDeCaracteristicas'];
        }
    });

    $.each(tiposDeCaracteristicas, function (i, value) {
        AddNewCharacteristic(value['Id'], value['Nombre']);
    });
}

function ObtenerTiposDeRecursos() {
    $.ajax({
        url: '/Recursos/ObtenerTiposDeRecursosYCaracteristicas',
        async: false,
        contentType: 'application/html; charset=utf-8',
        type: 'GET',
        data: { tipoId: $('#TipoId').val() },
        dataType: 'json'
    }
    ).success(function (result) {
        tiposDeRecursosJSON = result;
    });
};

function ObtenerTodasCaracteristicas(url) {
    $.ajax({
        url: url,
        async: false,
        contentType: 'application/html; charset=utf-8',
        type: 'GET',
        dataType: 'json'
    }
    ).success(function (result) {
        tiposDeCaracteristicas = result;
    });
}

function ObtenerRecursos(url) {
    $.ajax({
        url: url,
        async: false,
        type: 'POST',
        data: $("form[action='" + url + "'").serialize()

    }).success(function (result) {
        //$('#resultadoBusqueda').html(JSON.stringify(result));
        MostrarResultados(result);
    });
}

function AddNewCharacteristic(tipoId, characteristicName) {
    // Get template from html
    var characteristicTemplate = $('#characteristic-template').html();

    // Replace content
    characteristicTemplate = replaceAll('{{TipoId}}', tipoId, characteristicTemplate);
    characteristicTemplate = replaceAll('{{CharacteristicName}}', characteristicName, characteristicTemplate);

    $('#characteristics-container').append(characteristicTemplate);
}

function UpdateCaracteristicasNames() {
    $("input[name^='CaracteristicasValor'").attr('name', 'CaracteristicasValor');
    $("select[name^='CaracteristicasTipo'").attr('name', 'CaracteristicasTipo');
}

function MostrarResultados(result) {
    $('#resultadoBusqueda').html('');
    $.each(result, function () {
        var template = $('#resource-template').html();

        template = replaceAll('{{CodigoRecurso}}', this['Codigo'], template);
        template = replaceAll('{{NombreRecurso}}', this['Nombre'], template);

        $('#resultadoBusqueda').append(template);
    });
}