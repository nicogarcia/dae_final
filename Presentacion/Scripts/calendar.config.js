﻿
$(function() {
    $('#calendar').fullCalendar({
        header: {
            left: 'prev,next today',
            center: 'title',
            right: 'month,basicWeek,basicDay'
        },
        height: 500,
        timeFormat: 'H:mm',
        fixedWeekCount: false,
        hiddenDays: [0],
        editable: true,
        eventLimit: true, // allow "more" link when too many events
    });
});

function LoadCurrentView() {
    if (localStorage.getItem("vistaTabla")) {
        $("#vistaTabla").css('visibility', localStorage.getItem("vistaTabla"));
        var tablaDisplay = localStorage.getItem("vistaTabla") == 'hidden' ? 'none' : 'visible';
        $("#vistaTabla").css('display', tablaDisplay);
        $("#calendar").css('visibility', tablaDisplay == 'none' ? 'visible' : 'hidden');
    }
}

function ToggleReservasView() {
    ToggleVisibility($('#vistaTabla'));
    $('#vistaTabla').toggle();
    ToggleVisibility($('#calendar'));
}

function ToggleVisibility(object) {
    if ($(object).css('visibility') == 'hidden') {
        $(object).css('visibility', 'visible');
        localStorage.setItem(object.selector.slice(1), 'visible');
    } else {
        $(object).css('visibility', 'hidden');
        localStorage.setItem(object.selector.slice(1), 'hidden');
    }
}
