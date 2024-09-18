

function showHidePasswordField(fieldId)
{
    $(`#${fieldId} a`).on('click', function(event) {
        event.preventDefault();
        console.log(fieldId)
        if($(`#${fieldId} input`).attr("type") === "text")
        {
            $(`#${fieldId} input`).attr('type', 'password');
            $(`#${fieldId} i`).addClass( "fa-eye-slash" );
            $(`#${fieldId} i`).removeClass( "fa-eye" );
        }
        else if($(`#${fieldId} input`).attr("type") === "password")
        {
            $(`#${fieldId} input`).attr('type', 'text');
            $(`#${fieldId} i`).removeClass( "fa-eye-slash" );
            $(`#${fieldId} i`).addClass( "fa-eye" );
        }
    });

}