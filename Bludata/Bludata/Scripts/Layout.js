// Mostra o panel de sucesso/erro.
function MostrarAlerta(sucesso, mensagem) {
    $('#pnlAlerta').removeClass("alert-danger").removeClass("alert-success");

    if (sucesso) {
        $('#pnlAlerta').addClass("alert-success");
    }
    else {
        $('#pnlAlerta').addClass("alert-danger");
    }

    $('#pnlAlerta span').html(mensagem);
    $('#pnlAlerta').show();
}

$('#btnSC').on("click", function () {
    setCookie("UNIDADE_FEDERATIVA", "SC", 1);

    // escondo a modal de escolha da unidade federativa e mostra o input de RG
    $('#rgContent').show();
    $('#ufContent').hide();
});

$('#btnPR').on("click", function () {
    setCookie("UNIDADE_FEDERATIVA", "PR", 1);

    // limpa o valor do rg e o esconde
    $('.pessoaRG').val('');
    $('#rgContent').hide();

    // escondo a modal de escolha da unidade federativa
    $('#ufContent').hide();
});

$('#MudarUnidadeFederativa').on("click", function () {
    // mostra a modal de escolha da unidade federativa
    $('#ufContent').show();
});

$(document).ready(function () {
    var unidadeFederativa = getCookie("UNIDADE_FEDERATIVA");

    // Verifica se o usuário já informou qual a sua unidade federativa.
    if (unidadeFederativa === null || typeof (unidadeFederativa) === 'undefined') {
        $('#ufContent').show();
    }

});

// Adiciona cookie ao navegador.
function setCookie(cname, cvalue, exdays) {
    var d = new Date();
    d.setTime(d.getTime() + (exdays * 24 * 60 * 60 * 1000));
    var expires = "expires=" + d.toUTCString();
    document.cookie = cname + "=" + cvalue + ";" + expires + ";path=/";
}

// Recupera o cookie do navegador.
function getCookie(cname) {
    var name = cname + "=";
    var decodedCookie = decodeURIComponent(document.cookie);
    var ca = decodedCookie.split(';');
    for (var i = 0; i < ca.length; i++) {
        var c = ca[i];
        while (c.charAt(0) == ' ') {
            c = c.substring(1);
        }
        if (c.indexOf(name) == 0) {
            return c.substring(name.length, c.length);
        }
    }
    return null;
}