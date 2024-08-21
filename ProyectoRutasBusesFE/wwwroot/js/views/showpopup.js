function ShowPopup(url) {
    var $modal = $('#modalDefault');
    $modal.find('.modal-content').load(url, function () {
        $modal.modal('show');
    });
}

function SubmitAddEdit(form) {
    var $form = $(form);
    var url = $form.attr('action');
    var data = $form.serialize();

    $.post(url, data)
        .done(function (response) {
            $('#modalDefault').modal('hide');
            location.reload(); // Recarga la página para mostrar los cambios
        })
        .fail(function (response) {
            alert('Error al guardar los datos. Por favor, intente nuevamente.');
        });

    return false; // Prevenir el envío normal del formulario
}
