
function sortTable(columnIndex) {
    var table, rows;
    table = document.getElementById("customers_table").getElementsByTagName('table')[0];
    rows = Array.from(table.rows).slice(1); // Convert HTMLCollection to array and remove header row

    rows.sort(function(a, b) {
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
