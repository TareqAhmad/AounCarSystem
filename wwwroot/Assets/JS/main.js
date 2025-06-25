
/**
 * Dealing With Login 
 */
$(document).ready(function () {
    
}); 

function login() {
        const username = $("#username").val();
        const password = $("#password").val();

        console.log(username);
        console.log(password);

        $.ajax({
            type: 'POST',
            url: '/Login/Authenticate',
            data: {
                username: username,
                password: password
            },
            success: function (response) {
                if (response.success) {
                    window.location.href = "/Pages/Dashboard.html";
                } else {
                    $("#message").text("اسم المستخدم او كلمة السر غير صحيحة").css("color", "red");
                }
            },
            error: function () {
                $("#message").text("Error contacting server").css("color", "orange");
            }
        });
 }


/**
 *  Header Menu
 */
function toggleMenu() {
    document.getElementById('nav').classList.toggle('active');
}

// Handle dropdowns in mobile
document.addEventListener('DOMContentLoaded', () => {
    const dropdownLinks = document.querySelectorAll('.dropdown > a');

    dropdownLinks.forEach(link => {
        link.addEventListener('click', e => {
            if (window.innerWidth <= 768) {
                e.preventDefault();
                link.parentElement.classList.toggle('active');
            }
        });
    });
});


    /**
     *  Dealing With Cars 
     */

$(document).ready(function () {
    loadCars();

function loadCars() {
    $.ajax({
        type: 'Get',
        url: '/Cars/Get_AllCars',
        success: function (Cars) {
            $('#car-table-body').empty(); 
            $.each(Cars, function (i, Car) {

                $('#car-table-body').append(`         
                
                <tr>
                        <td>${Car.chassisNumber}</td>
                        <td>${Car.carName}</td>
                        <td>${Car.carType}</td>
                        <td>${Car.carModel}</td>
                        <td>${Car.carPrice}</td>
                        <td>${Car.status}</td>
                        <td>${Car.supplierName}</td>
                        <td>
                            <button class ="btn btn-sm btn-primary" onclick="EditCar(${Car.chassisNumber})">Update</button>
                            <button class ="btn btn-sm btn-danger" onclick = "DeleteCar(${Car.chassisNumber})">Delete</button>
                        </td>

                </tr>`);
            });

        },
        error: function () {
            alert('Failed to laod Cars'); 
        }
    });
    };

    function EditCar() { };

    function DeleteCar() { };

});


/**
 *  Dealing With Users
 */

$(document).ready(function () {
   LoadUsers();


function LoadUsers() {

    $.ajax({
        type: 'GET',
        url: '/Users/Get_AllUsers',
        success: function (Users) {
            $('#user-table-body').empty();
            $.each(Users, function (i, user) {

                $('#user-table-body').append(`
                    
                   <tr>
                      <td>${user.user_Id}</td>
                      <td>${user.username}</td>
                      <td>${user.password}</td>
                      <td>${user.permissions}</td>
                      <td>                
                        <button class="btn btn-sm btn-primary" onclick="EditUser(${user.user_id})">Update</button>
                        <button class="btn btn-sm btn-danger" onclick="DeleteUser(${user.user_id})">Delete</button>
                      </td>
                  </tr>`);


            });
        },
        error: function () {
            alert('Failed to load Users')
        }


    });





};

function EditUsers() {
    console.log('Edit User');
};

function DeleteUser() {
    console.log('Delete User');
};
});

/**
 * Dealing With Customers  
 */

$(document).ready(function () {
    LoadCustomers();

function LoadCustomers() {

    $.ajax({
        type: 'GET',
        url: '/Customers/Get_AllCustomers',
        success: function (Customers) {
            $("#customer-table-body").empty();
            $.each(Customers, function (i, cust) {

                $("#customer-table-body").append(`
                  
                   <tr>
                       <td>${cust.cust_Id}</td>
                       <td>${cust.fullName}</td>
                       <td>${cust.phone}</td>
                       <td>${cust.email}</td>
                       <td>${cust.address}</td>
                       <td>${cust.nationalID}</td>
                       <td>
                            <button class="btn btn-sm btn-primary" onclick="EditCustomer(${cust.cust_Id})">Update</button>
                            <button class="btn btn-sm btn-danger" onclick="DeleteCustomer(${cust.cust_Id})">Delete</button>
                      </td>

                   </tr>

                `)


            });

        },
        error: function () {
            alert('Failed to load Customers');
        }



    }); 


};
function EditCustomer() { }; 
function DeleteCustomer() { };

}); 


/**
 *  Dealing With Suppliers
 */

$(document).ready(function () {
    LoadSuppliers();


function LoadSuppliers() {

    $.ajax({
        type: 'GET',
        url: '/Suppliers/Get_AllSuppliers',
        success: function (Suppliers) {
            $('#supplier-table-body').empty();
            $.each(Suppliers, function (i, sup) {

                $('#supplier-table-body').append(`
                    
                   <tr>
                      <td>${sup.supplier_Id}</td>
                      <td>${sup.companyName}</td>
                      <td>${sup.phone}</td>
                      <td>${sup.email}</td>
                      <td>${sup.address}</td>
                      <td>                
                        <button class="btn btn-sm btn-primary" onclick="EditSupplier(${sup.supplier_Id})">Update</button>
                        <button class="btn btn-sm btn-danger" onclick="DeleteSupplier(${sup.supplier_Id})">Delete</button>
                      </td>
                  </tr>`);
            });
        },
        error: function () {
            alert('Failed to load Suppliers')
        }


    });

}
});

/**
 *   Dealing With Sales
 */

$(document).ready(function () {
    LoadMasterInvoices(); 
   
function LoadMasterInvoices()
{
    $.ajax({
        type: 'GET',
        url: '/Sales/Get_MasterInvoices',
        success: function (Masters) {
            $("#MasterInvoice-table-body").empty(); 
            $.each(Masters, function (i, master) {

                $("#MasterInvoice-table-body").append(`
                   <tr>
                        <td>${master.inv_Id}</td>
                        <td>${master.inv_Value}</td>
                        <td>${master.inv_date}</td>
                        <td>${master.fullName}</td>
                        <td>
                          <button class="btn btn-sm btn-success btn-view" onclick="ViewDetails(${master.inv_Id})">View Detials</button>
                          <button class="btn btn-sm btn-primary" onclick="EditInvoice(${master.inv_Id})">Update</button>
                          <button class="btn btn-sm btn-danger" onclick="DeleteIvoice(${master.inv_Id})">Delete</button>
                        </td>
                   </tr>
                `);

            })
        },
        error: function () {
            alert('Failed to load Master Invoices')
        }


    });

}

    function ViewDetails(Inv_id)
    {
    console.log(Inv_id);
    $.ajax({
        type: 'GET',
        url: '/Sales/Get_DetailsInvoice',
        data: {
            Inv_id : Inv_id
        },
        success: function (Details) {
            $('#DetailInvoice-table-body').empty(); 
            $.each(Details, function (i,detail) {

                $('#DetailInvoice-table-body').append(`

                  <tr>
                       <td>${detail.inv_Id}</td>
                       <td>${detail.chassisNumber}</td>
                       <td>${detail.carName}</td>
                       <td>${detail.carPrice}</td>
                       <td>${detail.quantity}</td>
                  </tr>
                `);

            });



        },
        error: function () { }

    });

}

function EditInvoice() { }

function DeleteInvoice() { }

});


/**
 *  Dealing With Expenses
 */

$(document).ready(function () {
    LoadExpenses();
    function LoadExpenses() {

        $.ajax({
            type: 'GET',
            url: '/Expenses/Get_AllExpenses',
            success: function (Expenses) {
                $('#expense-table-body').empty(); 
                $.each(Expenses, function (i, exp) {
               
                    $('#expense-table-body').append(`
                       <tr>
                          <td>${exp.expense_Id} </td>
                          <td>${exp.expenseType}</td>
                          <td>${exp.amount}</td>
                          <td>${exp.expenseDate}</td>
                          <td>${exp.description}</td>
                          <td>${exp.chassisNumber}</td>
                          <td>
                              <button class="btn btn-sm btn-primary" onclick="EditExpense(${exp.expense_Id})">Update</button>
                              <button class="btn btn-sm btn-danger" onclick="DeleteExpense(${exp.expense_Id})">Delete</button>
                          </td>

                       </tr>
                    `);


                });



            },
            error: function () {
                alert('Failed to load Expenses'); 
            }




        });
    }

});

/**
 *  Dealing With Costs
 */

$(document).ready(function () {

    LoadCosts(); 
    function LoadCosts() {
        $.ajax({
            type: 'GET',
            url: '/Costs/Get_AllCosts',
            success: function (Costs) {
                $('#cost-table-body').empty();
                $.each(Costs, function (i, cost) {

                    $('#cost-table-body').append(`
                   <tr>
                      <td>${cost.cost_Id} </td>
                      <td>${cost.costName}</td>
                      <td>${cost.costValue}</td>
                      <td>${cost.chassisNumber}</td>
                      <td>
                          <button class="btn btn-sm btn-primary" onclick="EditCost(${cost.cost_Id})">Update</button>
                          <button class="btn btn-sm btn-danger" onclick="DeleteCost(${cost.cost_Id})">Delete</button>
                      </td>

                   </tr> `
                    );


                });
            },
                    error: function () {
                        alert('Failed to load Costs');
                    }
        });


    }




});














