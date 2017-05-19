var telefoneMask = "(#00) #0000-0000";

$('#btnAddTelefone').on("click", function ()
{
    $('.TelefoneInput:first').unmask();
    $telefone = $('.TelefoneInput:first').clone(true);
    
    // Adiciona um novo input para telefone
    $telefone.appendTo(".DataContent");
    $('.TelefoneInput:last').val("");    
    $('.TelefoneInput:last').mask(telefoneMask);

    $('.TelefoneInput:first').mask(telefoneMask);
});

$('.TelefoneInput').on("focusin", function () {
    $(this).mask(telefoneMask);
});

$('#btnRemoveTelefone').on("click", function () {

    // Verifica se não é o último input de telefone e remove
    if ($('.TelefoneInput').length > 1) {
        $telefone = $('.TelefoneInput:last');
        $telefone.remove();
    }
});

// Define as mácaras para os campos de cadastro
$('#CPF').mask('000.000.000-00');
$('#DataNascimento').mask('00/00/0000');

// Define o tamanho máximo dos inputs
$("#CPF").attr("maxlength", 15);
$("#RG").attr("maxlength", 15);
$("#Nome").attr("maxlength", 50);