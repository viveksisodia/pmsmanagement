
app.factory("customerService", function ($http) {
    return {

        getCustomers: function () {
            return $http({
                method: "POST",
                url: "/customer/default.aspx/GetCustomers",
                headers: { "Content-Type": "application/json" },
                data: JSON.stringify({ CustomerID: "1" })
            });
        },

        getCustomerById: function (id) {
            return $http({
                method: "POST",
                url: "/customer/default.aspx/GetCustomersbyid",
                headers: { "Content-Type": "application/json" },
                data: JSON.stringify({ CustomerID: id })
            });
        },

        saveindustry: function (model) {
            return $http({
                method: "POST",
                url: "/customer/default.aspx/Addindustry",
                headers: { "Content-Type": "application/json" },
                data: JSON.stringify({ model: model })
            });
        },
        checkindustry: function (name) {           
            return $http({
                method: "POST",
                url: "/customer/default.aspx/checkindustry",
                headers: { "Content-Type": "application/json" },
                data: JSON.stringify({ name: name })
            });
        }
    };
});
