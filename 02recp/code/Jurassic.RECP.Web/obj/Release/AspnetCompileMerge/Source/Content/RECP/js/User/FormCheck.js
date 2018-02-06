
function updateError(e, validGroupName) {
    var control = e.sender;
    removeError(control);
    if (e.isValid == false) {
        addError(control, e.errorText, validGroupName);
    }
}
function focusInput(id) {
    var el = document.getElementById(id);
    if (el) el.focus();
}
function addError(control, errorText, validGroupName) {
    var textId = control.id + "$text";
    var errorId = control.uid + "$errorText";
    removeError(control);
    var jq = jQuery("#" + validGroupName);
    jq.append('<li id="' + errorId + '"><a href="javascript:focusInput(\'' + textId + '\')">' + errorText + '</a></li>');
}
function removeError(control) {
    var errorId = control.uid + "$errorText";
    var el = document.getElementById(errorId);
    jQuery(el).remove();
}