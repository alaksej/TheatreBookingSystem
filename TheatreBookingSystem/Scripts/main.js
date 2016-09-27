(function() {
    var inputs = document.getElementsByClassName('text-box single-line');
    var td = document.getElementById("quantity-total");

    var sum = 0;

    bindEvents();
    calcSum();

    function bindEvents() {
        for (var i = 0; i < inputs.length; i++) {
            inputs[i].addEventListener('change', calcSum);
            inputs[i].addEventListener('keyup', calcSum);
            inputs[i].addEventListener('focus', selectContent);
        }
    }

    function calcSum() {
        for (var i = 0; i < inputs.length; i++) {
            sum += parseInt(inputs[i]["value"]);
        }
        render();
    }

    function selectContent() {
        this.select();
    }

    function render() {
        td.innerHTML = sum;
        sum = 0;
    }
})();