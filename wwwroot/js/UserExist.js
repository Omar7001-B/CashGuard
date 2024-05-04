$(function () {
    setTimeout(function () {
        $('body').removeClass('loading');
    }, 1000);

    // Function to handle button click event
    $('.go-back-btn').on('click', function () {
        window.location.href = "/Login/index"; // Replace "your_specific_url_here" with the URL you want to go back to
    });
});
