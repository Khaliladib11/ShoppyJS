const rangeInputs = () => {

    const minPrice = document.getElementById('min_Price');
    const maxPrice = document.getElementById('max_Price');
    const minValue = document.getElementById('min_value');
    const maxValue = document.getElementById('max_value');

    minValue.innerHTML = minPrice.value;
    maxValue.innerHTML = maxPrice.value;

    minPrice.oninput = function () {
        minValue.innerHTML = this.value;
    }
    maxPrice.oninput = function () {
        maxValue.innerHTML = this.value;
    }
}

rangeInputs();

