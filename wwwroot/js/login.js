const signUpButton = document.getElementById('signUp');
const signInButton = document.getElementById('signIn');
const container = document.getElementById('container');
function previewPhoto(input) {
    if (input.files && input.files[0]) {
        var reader = new FileReader();
        reader.onload = function (e) {
            document.getElementById('photoPreview').style.backgroundImage = 'url(' + e.target.result + ')';
            document.getElementById('photoPath').value = e.target.result; // Set photo path to hidden input
        };
        reader.readAsDataURL(input.files[0]);
    }
}


signUpButton.addEventListener('click', () => {
	container.classList.add("right-panel-active");
});

signInButton.addEventListener('click', () => {
	container.classList.remove("right-panel-active");
});