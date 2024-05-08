let listContainer = document.getElementById('list-container')
let head = document.querySelector('head')
let inputBox = document.getElementById('input-box')


function addTask() {
    if (inputBox.value == '') {
        alert('Please Enter the Text')
    }
    else {
        const task = document.createElement('li')
        task.textContent = inputBox.value;
        listContainer.appendChild(task)
        // inputBox.value = ''
        let span = document.createElement('span')
        span.textContent = "\u00d7"
        task.appendChild(span)
        span.style.right = '0px';
    }
    inputBox.value = '';
    saveData()
}

listContainer.addEventListener('click', (e) => {
    if (e.target.tagName === 'LI') {
        e.target.classList.toggle("checked")
        saveData()
    }
    else if (e.target.tagName === 'SPAN') {
        e.target.parentElement.remove()
        saveData()
    }
})

function saveData() {
    localStorage.setItem("data", listContainer.innerHTML)
}

function showTask() {
    listContainer.innerHTML = localStorage.getItem("data");
}

showTask()
function sortTable(columnIndex) {
    var table, rows;
    table = document.getElementById("customers_table").getElementsByTagName('table')[0];
    rows = Array.from(table.rows).slice(1); // Convert HTMLCollection to array and remove header row

    rows.sort(function (a, b) {
        var x = a.getElementsByTagName("td")[columnIndex].innerHTML.toLowerCase();
        var y = b.getElementsByTagName("td")[columnIndex].innerHTML.toLowerCase();
        return x.localeCompare(y);
    });

    var arrowIcon = table.getElementsByTagName('th')[columnIndex].querySelector('.icon-arrow');

    if (arrowIcon.innerHTML === '▲') {
        rows.reverse(); // If already sorted in ascending order, reverse to get descending
        arrowIcon.innerHTML = '▼';
    } else {
        arrowIcon.innerHTML = '▲';
    }

    // Reinsert sorted rows into table
    for (var i = 0; i < rows.length; i++) {
        table.appendChild(rows[i]);
    }
}
