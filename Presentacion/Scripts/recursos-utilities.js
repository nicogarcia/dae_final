var tiposDeRecursosJSON;



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