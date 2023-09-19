
async function fetchAndDisplayData() {
    try {
        const response = await fetch('https://localhost:7065/api/Customers/GetCustomerDetails');
        const data = await response.json();
        const tableBody = document.querySelector('#data-table');

        tableBody.innerHTML = ''; // Clear existing table rows

        data.forEach(customer => {
            const row = document.createElement('tr');
            row.innerHTML = `
                <td class="custom-table-cell">${customer.Customer_ID}</td>
                <td class="custom-table-cell">${customer.First_Name}</td>
                <td class="custom-table-cell">${customer.Last_Name}</td>
                <td class="custom-table-cell">${customer.Address}</td>
                <td class="custom-table-cell">${customer.City}</td>
            `;
            tableBody.appendChild(row);
        });
    } catch (error) {
        console.error('Error fetching data:', error);
    }
}

// Function to call when DOM content is fully loaded
function initialize() {
    fetchAndDisplayData();

}

// Load the data when the DOM content is fully loaded
document.addEventListener('DOMContentLoaded', initialize);
