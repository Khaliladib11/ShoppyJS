

const plusIcon = document.getElementById('plus-icon');
const subIcon = document.getElementById('sub-icon');
var nb = parseInt(parseInt(document.getElementById('number').innerHTML, 10));

plusIcon.addEventListener('click', function () {
    nb = isNaN(nb) ? 0 : nb;
    nb++;
    document.getElementById('number').innerHTML = nb;
});
subIcon.addEventListener('click', function () {
    nb = isNaN(nb) ? 0 : nb;
    if (nb > 0) {
        nb--;
    }
    document.getElementById('number').innerHTML = nb;
});

