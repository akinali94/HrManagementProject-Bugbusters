$(document).ready(function () {
    $('.allowance-wrapper').hover(
        function () {
            // Mouse enters the allowance-wrapper
            $(this).find('.allowanceSubmenu').show();
        },
        function () {
            // Mouse leaves the allowance-wrapper
            $(this).find('.allowanceSubmenu').hide();
        }
    );
});
