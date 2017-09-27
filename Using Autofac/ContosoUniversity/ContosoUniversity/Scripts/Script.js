$(function () {
    $(".date-picker").datepicker(
    {
        changeMonth : true,
        changeYear: true,
        yearRange: "-100:+0",
        dateFormat: "dd/mm/yy"
    });
});