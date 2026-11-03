 app.controller("customerCtrl", function ($scope, $http, $timeout) {

    function getHashParams() {
        let hash = window.location.hash || "";

        if (hash.startsWith("#")) {
            hash = hash.substring(1);
        }
        if (hash.startsWith("#")) {
            hash = hash.substring(1);
        }

        return new URLSearchParams(hash);
    }

    function setCustomHash(value) {
        window.location.hash = "##" + value;
    }

    //For popup master
    $scope.crmmasterdata = {
        name: "",
        Id: "0"
    }; 
    $scope.masterPopup = {
        visible: false,
        view: "",
        type: ""
    };
    $scope.openMasterPopup = function (type, name) {
      
        $scope.masterPopup.type = type;
        $scope.masterPopup.visible = true;
        $scope.masterPopup.title = "Industry";
        if (type === "industry") {     
          
            $scope.masterPopup.view = "/customer/views/customer-industry.html";
            $scope.masterIndustry = {};
          //  $scope.crmmasterdata.name = name;
            $scope.focusIndustryInput = true;
            $scope.newCustomer.TxtIndustry = "";
          
        }
    };
    $scope.closeMasterPopup = function () {
      
        $scope.masterPopup.visible = false;
        $scope.masterPopup.view = "";
        $scope.crmmasterdata.name = "";
        $scope.crmmasterdata.Id = "0";
        $scope.focusIndustryInput = false;
        document.getElementById("closepopupidhere").click();

    };
     //END For popup master

      //For saveIndustry
    $scope.saveIndustry = function () {
        customerService.insertIndustry($scope.masterIndustry).then(function (response) {
            $scope.closeMasterPopup();
            $scope.loadIndustries();

            if (response.data.d) {
                $scope.newCustomer.IndustryId = response.data.d.Id || response.data.d;
            }
        });
    };

   
 
    $scope.view = 'table';

    let params = getHashParams();

    if (params.get("mode")) {
        let id = params.get("id");

        if (id) {
            $timeout(function () {
                $scope.loadCustomerbyid(id);
                $scope.showlist = false;
                $scope.showAddForm = true;
            }, 0);
        } else {
            $timeout(function () {
                $scope.showlist = false;
                $scope.newCustomer = angular.copy($scope.defaultCustomer);
                $scope.newCustomer.QType = "insert";
                $scope.showAddForm = true;
            }, 0);
        }
    }
    else {
        $scope.view = params.get("viewtype") || "table";
        $scope.setView = function (mode) {
            $scope.view = mode;
            setCustomHash("viewtype=" + mode);
        };
    }

    $scope.highlightMatch = function (text, search) {
        if (!search || search.trim() === "") return text;
        let pattern = new RegExp("(" + search + ")", "gi");
        return text.replace(pattern, "<b>$1</b>");
    };

    $scope.currentPage = 1;
    $scope.countryid = 1;
   
    $scope.loadcountry = function () {        
        $http({
            method: "POST",
            url: "/customer/default.aspx/Getcountryid",
            headers: { "Content-Type": "application/json" },
            data: JSON.stringify({ regid: "1" })
        }).then(function (response) {
            $scope.countryid = response.data.d;
            $scope.VATTreatmenttext = ($scope.countryid == 1) ? "GST Treatment" : "VAT Treatment";
            $scope.lblvattrnnumber = ($scope.countryid == 1) ? "GSTN Number" : "VAT TRN Number";
        }).catch(function (error) {
            debugger;
            alert("HTTP ERROR: " + (error.statusText || error.message || "Unknown error"));
            console.error("HTTP ERROR:", error);
        }).finally(function () {
            
        });
    };
    
    $scope.loadcountry(); 
    $scope.customers = [];
    $scope.txtsalevalueprvyear = "";
    $scope.searchText = "";
    $scope.sortColumn = "Id";
    $scope.reverseSort = false;
    $scope.pageSizes = [10, 20, 50, 100];
    $scope.types = ["1", "2"];
    $scope.pageSize = 10;

    $scope.loadCustomers = function () {
        $scope.isLoading = true;
        $http({
            method: "POST",
            url: "/customer/default.aspx/GetCustomers",
            headers: { "Content-Type": "application/json" },
            data: JSON.stringify({ CustomerID: $scope.customertype || "1" })
        }).then(function (response) {
            $scope.customers = response.data.d;
        }).catch(function (error) {
            debugger;
            alert("HTTP ERROR: " + (error.statusText || error.message || "Unknown error"));
            console.error("HTTP ERROR:", error);
        }).finally(function () {
            $scope.isLoading = false;
        });
    };

    $scope.loadCustomers();

    $scope.sortData = function (column) {
        $scope.reverseSort = ($scope.sortColumn == column) ? !$scope.reverseSort : false;
        $scope.sortColumn = column;
    };

    $scope.getSortClass = function (column) {
        if ($scope.sortColumn == column) {
            return $scope.reverseSort ? "↓" : "↑";
        }
        return "";
    };

    $scope.editField = function (row, field) {
        row.backup = angular.copy(row);
        row["edit" + field] = true;
    };

    $scope.saveField = function (row, field) {
        row["edit" + field] = false;
    };

    $scope.keyAction = function (event, row, field) {
        if (event.keyCode === 13) {
            $scope.saveField(row, field);
        }
        if (event.keyCode === 27) {
            angular.copy(row.backup, row);
            row["edit" + field] = false;
        }
    };

    // Currency list
    $scope.currencyList = [];

    $scope.loadcurrencylist = function () {
        $http({
            method: "POST",
            url: "/customer/default.aspx/getcurrencylist",
            headers: { "Content-Type": "application/json" },
            data: JSON.stringify({ regid: "1" })
        }).then(function (response) {
            $scope.currencyList = response.data.d;
        }).catch(function (error) {
            debugger;
            alert("HTTP ERROR: " + (error.statusText || error.message || "Unknown error"));
            console.error("HTTP ERROR:", error);
        });
    };

    $scope.loadcurrencylist();

    $scope.currencySearch = "";   
    $scope.TxtCurrency = "";
    $scope.showCurrencyDropdown = false;
    $scope.currencyHighlightIndex = -1;

    $scope.filterCurrency = function (item) {
        if (!$scope.newCustomer.currencySearch) return true;
        let q = $scope.newCustomer.currencySearch.toLowerCase();
        return (item.name).toLowerCase().includes(q);
    };

    $scope.selectCurrency = function (c) {
        $scope.newCustomer.TxtCurrency = c.Id;
        $scope.newCustomer.currencySearch = c.name;
        $scope.showCurrencyDropdown = false;
        $scope.currencyHighlightIndex = -1;
        if ($scope.newCustomer.QType == 'update') {
            $scope.saveCustomerauto();
        }
    };

    $scope.handleCurrencyKey = function (event) {
        let filtered = $scope.currencyList.filter($scope.filterCurrency);

        if (event.key === "ArrowDown") {
            event.preventDefault();
            if ($scope.currencyHighlightIndex < filtered.length - 1) {
                $scope.currencyHighlightIndex++;
            }
        }

        if (event.key === "ArrowUp") {
            event.preventDefault();
            if ($scope.currencyHighlightIndex > 0) {
                $scope.currencyHighlightIndex--;
            }
        }

        if (event.key === "Enter") {
            event.preventDefault();
            if ($scope.currencyHighlightIndex >= 0) {
                $scope.selectCurrency(filtered[$scope.currencyHighlightIndex]);
            }
        }
    };
    // Currency List END



    // salesperson list
    $scope.salespersonList = [];

    $scope.loadsalespersonlist = function () {
        $http({
            method: "POST",
            url: "/customer/default.aspx/getsalespersonlist",
            headers: { "Content-Type": "application/json" },
            data: JSON.stringify({ regid: "1" })
        }).then(function (response) {
            $scope.salespersonList = response.data.d;
        }).catch(function (error) {
            debugger;
            alert("HTTP ERROR: " + (error.statusText || error.message || "Unknown error"));
            console.error("HTTP ERROR:", error);
        });
    };

    $scope.loadsalespersonlist();

    $scope.salespersonSearch = "";
    $scope.Salepersonid = "";
    $scope.showsalespersonDropdown = false;
    $scope.salespersonHighlightIndex = -1;

    $scope.filtersalesperson = function (item) {
        if (!$scope.newCustomer.salespersonSearch) return true;
        let q = $scope.newCustomer.salespersonSearch.toLowerCase();
        return (item.name).toLowerCase().includes(q);
    };

    $scope.selectsalesperson = function (c) {
        $scope.newCustomer.Salepersonid = c.Id;
        $scope.newCustomer.salespersonSearch = c.name;
        $scope.showsalespersonDropdown = false;
        $scope.salespersonHighlightIndex = -1;
        if ($scope.newCustomer.QType == 'update') {
            $scope.saveCustomerauto();
        }
    };

    $scope.handlesalespersonKey = function (event) {
        let filtered = $scope.salespersonList.filter($scope.filtersalesperson);

        if (event.key === "ArrowDown") {
            event.preventDefault();
            if ($scope.salespersonHighlightIndex < filtered.length - 1) {
                $scope.salespersonHighlightIndex++;
            }
        }

        if (event.key === "ArrowUp") {
            event.preventDefault();
            if ($scope.salespersonHighlightIndex > 0) {
                $scope.salespersonHighlightIndex--;
            }
        }

        if (event.key === "Enter") {
            event.preventDefault();
            if ($scope.salespersonHighlightIndex >= 0) {
                $scope.selectsalesperson(filtered[$scope.salespersonHighlightIndex]);
            }
        }
    };
    // salesperson List END




    // modeofpayment list
    $scope.modeofpaymentList = [];

    $scope.loadmodeofpaymentlist = function () {
        $http({
            method: "POST",
            url: "/customer/default.aspx/getmodeofpaymentlist",
            headers: { "Content-Type": "application/json" },
            data: JSON.stringify({ regid: "1" })
        }).then(function (response) {
            $scope.modeofpaymentList = response.data.d;
        }).catch(function (error) {
            debugger;
            alert("HTTP ERROR: " + (error.statusText || error.message || "Unknown error"));
            console.error("HTTP ERROR:", error);
        });
    };

    $scope.loadmodeofpaymentlist();

    $scope.Txtmodeofpayment = "";
  
    $scope.Salepersonid = "";
    $scope.showmodeofpaymentDropdown = false;
    $scope.P_Txtmodeofpayment = "";
    $scope.P_showmodeofpaymentDropdown = false;
    $scope.modeofpaymentHighlightIndex = -1;

    $scope.filtermodeofpayment = function (item, Txtmodeofpayment) {
        if (!Txtmodeofpayment) return true;
        let q = Txtmodeofpayment.toLowerCase();
        return (item.name).toLowerCase().includes(q);
    };

    $scope.selectmodeofpayment = function (c) {
        
        $scope.newCustomer.Txtmodeofpayment = c.name;
        $scope.showmodeofpaymentDropdown = false;
        $scope.modeofpaymentHighlightIndex = -1;
        if ($scope.newCustomer.QType == 'update') {
            $scope.saveCustomerauto();
        }
    };
    $scope.P_selectmodeofpayment = function (c) {       
        $scope.newCustomer.P_Txtmodeofpayment = c.name;
        $scope.P_showmodeofpaymentDropdown = false;        
        if ($scope.newCustomer.QType == 'update') {
            $scope.saveCustomerauto();
        }
    };
    $scope.handlemodeofpaymentKey = function (event) {
        let filtered = $scope.modeofpaymentList.filter($scope.filtermodeofpayment);

        if (event.key === "ArrowDown") {
            event.preventDefault();
            if ($scope.modeofpaymentHighlightIndex < filtered.length - 1) {
                $scope.modeofpaymentHighlightIndex++;
            }
        }

        if (event.key === "ArrowUp") {
            event.preventDefault();
            if ($scope.modeofpaymentHighlightIndex > 0) {
                $scope.modeofpaymentHighlightIndex--;
            }
        }

        if (event.key === "Enter") {
            event.preventDefault();
            if ($scope.modeofpaymentHighlightIndex >= 0) {
                $scope.selectmodeofpayment(filtered[$scope.modeofpaymentHighlightIndex]);
            }
        }
    };
    // modeofpayment List END



    // Txtdeliverytype list
    $scope.TxtdeliverytypeList = [];

    $scope.loadTxtdeliverytypelist = function () {
        $http({
            method: "POST",
            url: "/customer/default.aspx/getTxtdeliverytypelist",
            headers: { "Content-Type": "application/json" },
            data: JSON.stringify({ regid: "1" })
        }).then(function (response) {
            $scope.TxtdeliverytypeList = response.data.d;
        }).catch(function (error) {
            debugger;
            alert("HTTP ERROR: " + (error.statusText || error.message || "Unknown error"));
            console.error("HTTP ERROR:", error);
        });
    };

    $scope.loadTxtdeliverytypelist();

    $scope.Txtdeliverytype = "";

    $scope.showTxtdeliverytypeDropdown = false;
    $scope.TxtdeliverytypeHighlightIndex = -1;

    $scope.filterTxtdeliverytype = function (item) {
        if (!$scope.newCustomer.Txtdeliverytype) return true;
        let q = $scope.newCustomer.Txtdeliverytype.toLowerCase();
        return (item.name).toLowerCase().includes(q);
    };

    $scope.selectTxtdeliverytype = function (c) {

        $scope.newCustomer.Txtdeliverytype = c.name;
        $scope.showTxtdeliverytypeDropdown = false;
        $scope.TxtdeliverytypeHighlightIndex = -1;
        if ($scope.newCustomer.QType == 'update') {
            $scope.saveCustomerauto();
        }
    };
    $scope.P_selectTxtdeliverytype = function (c) {

        $scope.newCustomer.P_Txtdeliverytype = c.name;
        $scope.P_showTxtdeliverytypeDropdown = false;
        $scope.P_TxtdeliverytypeHighlightIndex = -1;
        if ($scope.newCustomer.QType == 'update') {
            $scope.saveCustomerauto();
        }
    };
    $scope.handleTxtdeliverytypeKey = function (event) {
        let filtered = $scope.TxtdeliverytypeList.filter($scope.filterTxtdeliverytype);

        if (event.key === "ArrowDown") {
            event.preventDefault();
            if ($scope.TxtdeliverytypeHighlightIndex < filtered.length - 1) {
                $scope.TxtdeliverytypeHighlightIndex++;
            }
        }

        if (event.key === "ArrowUp") {
            event.preventDefault();
            if ($scope.TxtdeliverytypeHighlightIndex > 0) {
                $scope.TxtdeliverytypeHighlightIndex--;
            }
        }

        if (event.key === "Enter") {
            event.preventDefault();
            if ($scope.TxtdeliverytypeHighlightIndex >= 0) {
                $scope.selectTxtdeliverytype(filtered[$scope.TxtdeliverytypeHighlightIndex]);
            }
        }
    };
    // Txtdeliverytype List END


    // txtpterms list
    $scope.txtptermsList = [];

    $scope.loadtxtptermslist = function () {
        $http({
            method: "POST",
            url: "/customer/default.aspx/gettxtptermslist",
            headers: { "Content-Type": "application/json" },
            data: JSON.stringify({ regid: "1" })
        }).then(function (response) {
            $scope.txtptermsList = response.data.d;
        }).catch(function (error) {
            debugger;
            alert("HTTP ERROR: " + (error.statusText || error.message || "Unknown error"));
            console.error("HTTP ERROR:", error);
        });
    };

    $scope.loadtxtptermslist();

    $scope.txtpterms = "";
    $scope.Salepersonid = "";
    $scope.showtxtptermsDropdown = false;
    $scope.txtptermsHighlightIndex = -1;

    $scope.filtertxtpterms = function (item) {
        if (!$scope.newCustomer.txtpterms) return true;
        let q = $scope.newCustomer.txtpterms.toLowerCase();
        return (item.name).toLowerCase().includes(q);
    };

    $scope.selecttxtpterms = function (c) {
        
        $scope.newCustomer.txtpterms = c.name;
        $scope.showtxtptermsDropdown = false;
        $scope.txtptermsHighlightIndex = -1;
        if ($scope.newCustomer.QType == 'update') {
            $scope.saveCustomerauto();
        }
    };
    $scope.P_selecttxtpterms = function (c) {

        $scope.newCustomer.P_txtpterms = c.name;
        $scope.P_showtxtptermsDropdown = false;
        $scope.P_txtptermsHighlightIndex = -1;
        if ($scope.newCustomer.QType == 'update') {
            $scope.saveCustomerauto();
        }
    };
    $scope.handletxtptermsKey = function (event) {
        let filtered = $scope.txtptermsList.filter($scope.filtertxtpterms);

        if (event.key === "ArrowDown") {
            event.preventDefault();
            if ($scope.txtptermsHighlightIndex < filtered.length - 1) {
                $scope.txtptermsHighlightIndex++;
            }
        }

        if (event.key === "ArrowUp") {
            event.preventDefault();
            if ($scope.txtptermsHighlightIndex > 0) {
                $scope.txtptermsHighlightIndex--;
            }
        }

        if (event.key === "Enter") {
            event.preventDefault();
            if ($scope.txtptermsHighlightIndex >= 0) {
                $scope.selecttxtpterms(filtered[$scope.txtptermsHighlightIndex]);
            }
        }
    };
    // txtpterms List END


    // Txttransport list
    $scope.TxttransportList = [];

    $scope.loadTxttransportlist = function () {
        $http({
            method: "POST",
            url: "/customer/default.aspx/getTxttransportlist",
            headers: { "Content-Type": "application/json" },
            data: JSON.stringify({ regid: "1" })
        }).then(function (response) {
            $scope.TxttransportList = response.data.d;
        }).catch(function (error) {
            debugger;
            alert("HTTP ERROR: " + (error.statusText || error.message || "Unknown error"));
            console.error("HTTP ERROR:", error);
        });
    };

    $scope.loadTxttransportlist();

    $scope.Txttransport = "";
    $scope.Salepersonid = "";
    $scope.showTxttransportDropdown = false;
    $scope.TxttransportHighlightIndex = -1;

    $scope.filterTxttransport = function (item) {
        if (!$scope.newCustomer.Txttransport) return true;
        let q = $scope.newCustomer.Txttransport.toLowerCase();
        return (item.name).toLowerCase().includes(q);
    };

    $scope.selectTxttransport = function (c) {

        $scope.newCustomer.Txttransport = c.name;
        $scope.showTxttransportDropdown = false;
        $scope.TxttransportHighlightIndex = -1;
        if ($scope.newCustomer.QType == 'update') {
            $scope.saveCustomerauto();
        }
    };
    $scope.P_selectTxttransport = function (c) {

        $scope.newCustomer.P_Txttransport = c.name;
        $scope.P_showTxttransportDropdown = false;
        $scope.TxttransportHighlightIndex = -1;
        if ($scope.newCustomer.QType == 'update') {
            $scope.saveCustomerauto();
        }
    };
    $scope.handleTxttransportKey = function (event) {
        let filtered = $scope.TxttransportList.filter($scope.filterTxttransport);

        if (event.key === "ArrowDown") {
            event.preventDefault();
            if ($scope.TxttransportHighlightIndex < filtered.length - 1) {
                $scope.TxttransportHighlightIndex++;
            }
        }

        if (event.key === "ArrowUp") {
            event.preventDefault();
            if ($scope.TxttransportHighlightIndex > 0) {
                $scope.TxttransportHighlightIndex--;
            }
        }

        if (event.key === "Enter") {
            event.preventDefault();
            if ($scope.TxttransportHighlightIndex >= 0) {
                $scope.selectTxttransport(filtered[$scope.TxttransportHighlightIndex]);
            }
        }
    };
    // Txttransport List END


    // txtfreightterms list
    $scope.txtfreighttermsList = [];

    $scope.loadtxtfreighttermslist = function () {
        $http({
            method: "POST",
            url: "/customer/default.aspx/gettxtfreighttermslist",
            headers: { "Content-Type": "application/json" },
            data: JSON.stringify({ regid: "1" })
        }).then(function (response) {
            $scope.txtfreighttermsList = response.data.d;
        }).catch(function (error) {
            debugger;
            alert("HTTP ERROR: " + (error.statusText || error.message || "Unknown error"));
            console.error("HTTP ERROR:", error);
        });
    };

    $scope.loadtxtfreighttermslist();

    $scope.txtfreightterms = "";
    $scope.Salepersonid = "";
    $scope.showtxtfreighttermsDropdown = false;
    $scope.txtfreighttermsHighlightIndex = -1;

    $scope.filtertxtfreightterms = function (item) {
        if (!$scope.newCustomer.txtfreightterms) return true;
        let q = $scope.newCustomer.txtfreightterms.toLowerCase();
        return (item.name).toLowerCase().includes(q);
    };

    $scope.selecttxtfreightterms = function (c) {

        $scope.newCustomer.txtfreightterms = c.name;
        $scope.showtxtfreighttermsDropdown = false;
        $scope.txtfreighttermsHighlightIndex = -1;
        if ($scope.newCustomer.QType == 'update') {
            $scope.saveCustomerauto();
        }
    };
    $scope.P_selecttxtfreightterms = function (c) {

        $scope.newCustomer.P_txtfreightterms = c.name;
        $scope.P_showtxtfreighttermsDropdown = false;
        $scope.txtfreighttermsHighlightIndex = -1;
        if ($scope.newCustomer.QType == 'update') {
            $scope.saveCustomerauto();
        }
    };
    $scope.handletxtfreighttermsKey = function (event) {
        let filtered = $scope.txtfreighttermsList.filter($scope.filtertxtfreightterms);

        if (event.key === "ArrowDown") {
            event.preventDefault();
            if ($scope.txtfreighttermsHighlightIndex < filtered.length - 1) {
                $scope.txtfreighttermsHighlightIndex++;
            }
        }

        if (event.key === "ArrowUp") {
            event.preventDefault();
            if ($scope.txtfreighttermsHighlightIndex > 0) {
                $scope.txtfreighttermsHighlightIndex--;
            }
        }

        if (event.key === "Enter") {
            event.preventDefault();
            if ($scope.txtfreighttermsHighlightIndex >= 0) {
                $scope.selecttxtfreightterms(filtered[$scope.txtfreighttermsHighlightIndex]);
            }
        }
    };
    // txtfreightterms List END



    // txtSplInstructoin list
    $scope.txtSplInstructoinList = [];

    $scope.loadtxtSplInstructoinlist = function () {
        $http({
            method: "POST",
            url: "/customer/default.aspx/gettxtSplInstructoinlist",
            headers: { "Content-Type": "application/json" },
            data: JSON.stringify({ regid: "1" })
        }).then(function (response) {
            $scope.txtSplInstructoinList = response.data.d;
        }).catch(function (error) {
            debugger;
            alert("HTTP ERROR: " + (error.statusText || error.message || "Unknown error"));
            console.error("HTTP ERROR:", error);
        });
    };

    $scope.loadtxtSplInstructoinlist();

    $scope.txtSplInstructoin = "";
    $scope.Salepersonid = "";
    $scope.showtxtSplInstructoinDropdown = false;
    $scope.txtSplInstructoinHighlightIndex = -1;

    $scope.filtertxtSplInstructoin = function (item) {
        if (!$scope.newCustomer.txtSplInstructoin) return true;
        let q = $scope.newCustomer.txtSplInstructoin.toLowerCase();
        return (item.name).toLowerCase().includes(q);
    };

    $scope.selecttxtSplInstructoin = function (c) {

        $scope.newCustomer.txtSplInstructoin = c.name;
        $scope.showtxtSplInstructoinDropdown = false;
        $scope.txtSplInstructoinHighlightIndex = -1;
        if ($scope.newCustomer.QType == 'update') {
            $scope.saveCustomerauto();
        }
    };
    $scope.P_selecttxtSplInstructoin = function (c) {

        $scope.newCustomer.P_txtSplInstructoin = c.name;
        $scope.P_showtxtSplInstructoinDropdown = false;
        $scope.txtSplInstructoinHighlightIndex = -1;
        if ($scope.newCustomer.QType == 'update') {
            $scope.saveCustomerauto();
        }
    };
    $scope.handletxtSplInstructoinKey = function (event) {
        let filtered = $scope.txtSplInstructoinList.filter($scope.filtertxtSplInstructoin);

        if (event.key === "ArrowDown") {
            event.preventDefault();
            if ($scope.txtSplInstructoinHighlightIndex < filtered.length - 1) {
                $scope.txtSplInstructoinHighlightIndex++;
            }
        }

        if (event.key === "ArrowUp") {
            event.preventDefault();
            if ($scope.txtSplInstructoinHighlightIndex > 0) {
                $scope.txtSplInstructoinHighlightIndex--;
            }
        }

        if (event.key === "Enter") {
            event.preventDefault();
            if ($scope.txtSplInstructoinHighlightIndex >= 0) {
                $scope.selecttxtSplInstructoin(filtered[$scope.txtSplInstructoinHighlightIndex]);
            }
        }
    };
    // txtSplInstructoin List END


    // Account Ledger list
    $scope.AccountLedgerList = [];

    $scope.loadAccountLedgerList = function () {
        $http({
            method: "POST",
            url: "/customer/default.aspx/getAccountLedgerlist",
            headers: { "Content-Type": "application/json" },
            data: JSON.stringify({ regid: "1" })
        }).then(function (response) {
            $scope.AccountLedgerList = response.data.d;
          //  alert(response.data.d);
        }).catch(function (error) {
            debugger;
           // alert(error.statusText);
            alert("HTTP ERROR: " + (error.statusText || error.message || "Unknown error"));
            //console.error("HTTP ERROR:", error);
        });
    };

    $scope.loadAccountLedgerList();
    $scope.Txtaccountreceivable = "";
    $scope.accountreceivable = "0";
    $scope.showaccountreceivableDropdown = false;
    $scope.selectaccountreceivable = function (c) {
        $scope.newCustomer.accountreceivable = c.Id;
        $scope.newCustomer.Txtaccountreceivable = c.groupname;
        $scope.showaccountreceivableDropdown = false;
        if ($scope.newCustomer.QType == 'update') {
            $scope.saveCustomerauto();
        }
    };
    $scope.selectaccountpayable = function (c) {
        $scope.newCustomer.accountpayable = c.Id;
        $scope.newCustomer.Txtaccountpayable = c.groupname;
        $scope.showaccountpayableDropdown = false;
        if ($scope.newCustomer.QType == 'update') {
            $scope.saveCustomerauto();
        }
    };



   // Account Ledger List End



    // sectionList list
    $scope.sectionList = [];
    $scope.loadsectionList = function () {
        $http({
            method: "POST",
            url: "/customer/default.aspx/getsectionList",
            headers: { "Content-Type": "application/json" },
            data: JSON.stringify({ regid: "1" })
        }).then(function (response) {
            $scope.sectionList = response.data.d;
            //  alert(response.data.d);
        }).catch(function (error) {
            debugger;
            // alert(error.statusText);
            alert("HTTP ERROR: " + (error.statusText || error.message || "Unknown error"));
            //console.error("HTTP ERROR:", error);
        });
    };
    $scope.loadsectionList();
    $scope.Txtsectionid = "";
    $scope.sectionid = "0";
    $scope.showsectionidDropdown = false;
    $scope.selectsectionid = function (c) {
        $scope.newCustomer.sectionid = c.Id;
        $scope.newCustomer.Txtsectionid = c.name;
        $scope.showsectionidDropdown = false;
        if ($scope.newCustomer.QType == 'update') {
            $scope.saveCustomerauto();
        }
    };   
   // sectionList List End

    // VATTreatment  list
    $scope.VATTreatmentList = [];
    $scope.loadVATTreatmentList = function () {
        $http({
            method: "POST",
            url: "/customer/default.aspx/getVATTreatmentList",
            headers: { "Content-Type": "application/json" },
            data: JSON.stringify({ regid: "1" })
        }).then(function (response) {
            $scope.VATTreatmentList = response.data.d;
            //  alert(response.data.d);
        }).catch(function (error) {
            debugger;
            // alert(error.statusText);
            alert("HTTP ERROR: " + (error.statusText || error.message || "Unknown error"));
            //console.error("HTTP ERROR:", error);
        });
    };
    $scope.loadVATTreatmentList();
    $scope.TxtVATTreatment = "";
    $scope.VATTreatment = "0";
    $scope.showVATTreatmentDropdown = false;
    $scope.selectVATTreatment = function (c) {
        $scope.newCustomer.VATTreatment = c.Id;
        $scope.newCustomer.TxtVATTreatment = c.name;
        $scope.showVATTreatmentDropdown = false;
        if ($scope.newCustomer.QType == 'update') {
            $scope.saveCustomerauto();
        }
    };
   // VATTreatment List End

    // Parents list
    $scope.ParentsList = [];

    $scope.loadParentsList = function () {
        $http({
            method: "POST",
            url: "/customer/default.aspx/getParentslist",
            headers: { "Content-Type": "application/json" },
            data: JSON.stringify({ regid: "1" })
        }).then(function (response) {
            $scope.ParentsList = response.data.d;
        }).catch(function (error) {
            debugger;
            alert("HTTP ERROR: " + (error.statusText || error.message || "Unknown error"));
            console.error("HTTP ERROR:", error);
        });
    };

    $scope.loadParentsList();

    $scope.TxtParent = "";
    $scope.ParentCompany = "";
    $scope.showParentsDropdown = false;
    $scope.parentHighlightIndex = -1;

    $scope.filterParentsList = function (item) {
        if (!$scope.newCustomer.TxtParent) return true;
        let q = $scope.newCustomer.TxtParent.toLowerCase();
        return (item.name).toLowerCase().includes(q);
    };

    $scope.selectParentCompany = function (c) {
        $scope.newCustomer.ParentCompany = c.Id;
        $scope.newCustomer.TxtParent = c.name;
        $scope.showParentsDropdown = false;
        $scope.parentHighlightIndex = -1;
        if ($scope.newCustomer.QType == 'update') {
            $scope.saveCustomerauto();
        }
    };

    $scope.handleParentsListKey = function (event) {
        let filtered = $scope.ParentsList.filter($scope.filterParentsList);

        if (event.key === "ArrowDown") {
            event.preventDefault();
            if ($scope.parentHighlightIndex < filtered.length - 1) {
                $scope.parentHighlightIndex++;
            }
        }

        if (event.key === "ArrowUp") {
            event.preventDefault();
            if ($scope.parentHighlightIndex > 0) {
                $scope.parentHighlightIndex--;
            }
        }

        if (event.key === "Enter") {
            event.preventDefault();
            if ($scope.parentHighlightIndex >= 0) {
                $scope.selectParentCompany(filtered[$scope.parentHighlightIndex]);
            }
        }
    };

    // Industry list
    $scope.IndustrysList = [];

    $scope.loadIndustrysList = function () {
        $http({
            method: "POST",
            url: "/customer/default.aspx/getIndustryslist",
            headers: { "Content-Type": "application/json" },
            data: JSON.stringify({ regid: "1" })
        }).then(function (response) {
            $scope.IndustrysList = response.data.d;
        }).catch(function (error) {
            debugger;
            alert("HTTP ERROR: " + (error.statusText || error.message || "Unknown error"));
            console.error("HTTP ERROR:", error);
        });
    };

    $scope.loadIndustrysList();

    $scope.TxtIndustry = "";
    $scope.IndustryCompany = "";
    $scope.showIndustrysDropdown = false;
    $scope.industryTyping = false;
    $scope.industryHighlightIndex = -1;
    $scope.filterIndustrysList = function (item) {
        if (!$scope.industryTyping) {
            return true; // show all on click/edit open
        }
        if (!$scope.newCustomer.TxtIndustry) return true;
        let q = $scope.newCustomer.TxtIndustry.toLowerCase();
        return (item.name).toLowerCase().includes(q);
    };

    $scope.selectIndustryCompany = function (c) {
        $scope.newCustomer.IndustryCompany = c.Id;
        $scope.newCustomer.TxtIndustry = c.name;
        $scope.showIndustrysDropdown = false;
        $scope.industryHighlightIndex = -1;
        if ($scope.newCustomer.QType == 'update') {
            $scope.saveCustomerauto();
        }
    };

    $scope.handleIndustrysListKey = function (event) {
        let filtered = $scope.IndustrysList.filter($scope.filterIndustrysList);

        if (event.key === "ArrowDown") {
            event.preventDefault();
            if ($scope.industryHighlightIndex < filtered.length - 1) {
                $scope.industryHighlightIndex++;
            }
        }

        if (event.key === "ArrowUp") {
            event.preventDefault();
            if ($scope.industryHighlightIndex > 0) {
                $scope.industryHighlightIndex--;
            }
        }

        if (event.key === "Enter") {
            event.preventDefault();
            if ($scope.industryHighlightIndex >= 0) {
                $scope.selectIndustryCompany(filtered[$scope.industryHighlightIndex]);
            }
        }
    };

    // Country list
    $scope.CountryList = [];
    $scope.newAddress = {
        Billing: true,
        Shipping: false,
        LocationID: "",
        street1: "",
        street2: "",
        city: "",
        stateSearch: "",
        Txtstate: "",
        zipcode: "",
        TxtCountry: "",
        CountrySearch: "",
        id: 0
    };

    $scope.loadCountrylist = function () {
        $http({
            method: "POST",
            url: "/customer/default.aspx/getCountrylist",
            headers: { "Content-Type": "application/json" },
            data: JSON.stringify({ regid: "1" })
        }).then(function (response) {
            $scope.CountryList = response.data.d;
        }).catch(function (error) {
            debugger;
            alert("HTTP ERROR: " + (error.statusText || error.message || "Unknown error"));
            console.error("HTTP ERROR:", error);
        });
    };

    $scope.loadCountrylist();

    $scope.CountrySearch = "";
    $scope.TxtCountry = "";
    $scope.showCountryDropdown = false;
    $scope.countryHighlightIndex = -1;

    $scope.filterCountry = function (item) {
        if (!$scope.newAddress.CountrySearch) return true;
        let q = $scope.newAddress.CountrySearch.toLowerCase();
        return (item.name).toLowerCase().includes(q);
    };

    $scope.selectCountryaddress = function (c) {
        $scope.newAddress.TxtCountry = c.Id;
        $scope.newAddress.CountrySearch = c.name;
        $scope.showCountryDropdown = false;
        $scope.countryHighlightIndex = -1;
        $scope.loadstatelist();
    };

    $scope.handleCountryKey = function (event) {
        let filtered = $scope.CountryList.filter($scope.filterCountry);

        if (event.key === "ArrowDown") {
            event.preventDefault();
            if ($scope.countryHighlightIndex < filtered.length - 1) {
                $scope.countryHighlightIndex++;
            }
        }

        if (event.key === "ArrowUp") {
            event.preventDefault();
            if ($scope.countryHighlightIndex > 0) {
                $scope.countryHighlightIndex--;
            }
        }

        if (event.key === "Enter") {
            event.preventDefault();
            if ($scope.countryHighlightIndex >= 0) {
                $scope.selectCountry(filtered[$scope.countryHighlightIndex], $scope.newAddress);
            }
        }
    };

    $scope.$watch("newAddress.CountrySearch", function (newValue) {
        if (newValue === "" || newValue === null) {
            $scope.newAddress.stateSearch = "";
            $scope.newAddress.Txtstate = "";
            $scope.selectCountry({ Id: 0, name: "" }, $scope.newAddress);
            $scope.showCountryDropdown = true;
        }
    });

    $scope.checkSelectAllOrType = function (event) {
        let input = event.target;

        if (input.selectionStart === 0 &&
            input.selectionEnd === input.value.length &&
            input.value.length > 0) {

            $scope.filterCountry = function () {
                return true;
            };

            $scope.showCountryDropdown = true;
            $scope.$applyAsync();
            return;
        }

        $scope.filterCountry = function (item) {
            if (!$scope.newAddress.CountrySearch) return true;
            let q = $scope.newAddress.CountrySearch.toLowerCase();
            return item.name.toLowerCase().includes(q);
        };
    };

    $scope.selectCountry = function (c, addr) {
        addr.TxtCountry = c.Id;
        addr.CountrySearch = c.name;
        addr.showCountryDropdown = false;
        addr.highlightIndex = -1;
    };

    // State list
    $scope.stateList = [];

    $scope.loadstatelist = function () {
        if ($scope.newAddress.CountrySearch == "") {
            $scope.newAddress.TxtCountry = "";
        }

        $http({
            method: "POST",
            url: "/customer/default.aspx/getstatelist",
            headers: { "Content-Type": "application/json" },
            data: JSON.stringify({ regid: "1", countryid: $scope.newAddress.TxtCountry })
        }).then(function (response) {
            $scope.stateList = response.data.d;
        }).catch(function (error) {
            debugger;
            alert("HTTP ERROR: " + (error.statusText || error.message || "Unknown error"));
            console.error("HTTP ERROR:", error);
        });
    };

    $scope.loadstatelist();

    $scope.newAddress.stateSearch = "";
    $scope.newAddress.Txtstate = "";
    $scope.showstateDropdown = false;
    $scope.stateHighlightIndex = -1;

    $scope.filterstate = function (item) {
        if (!$scope.newAddress.stateSearch) return true;
        let q = $scope.newAddress.stateSearch.toLowerCase();
        return (item.name).toLowerCase().includes(q);
    };

    $scope.selectstateaddress = function (c) {
        $scope.newAddress.Txtstate = c.Id;
        $scope.newAddress.stateSearch = c.name;
        $scope.newAddress.TxtCountry = c.countryid;
        try {
            var curid = parseInt($scope.newAddress.TxtCountry);
            var result = $scope.CountryList.filter(function (x) {
                return x.Id == curid;
            });

            if (result.length > 0) {
                $scope.newAddress.CountrySearch = result[0].name;
            }
        } catch (error) {
            console.error("State select error:", error.message);
        }

        $scope.showstateDropdown = false;
        $scope.stateHighlightIndex = -1;
    };

    $scope.handlestateKey = function (event) {
        let filtered = $scope.stateList.filter($scope.filterstate);

        if (event.key === "ArrowDown") {
            event.preventDefault();
            if ($scope.stateHighlightIndex < filtered.length - 1) {
                $scope.stateHighlightIndex++;
            }
        }

        if (event.key === "ArrowUp") {
            event.preventDefault();
            if ($scope.stateHighlightIndex > 0) {
                $scope.stateHighlightIndex--;
            }
        }

        if (event.key === "Enter") {
            event.preventDefault();
            if ($scope.stateHighlightIndex >= 0) {
                $scope.selectstate(filtered[$scope.stateHighlightIndex], $scope.newAddress);
            }
        }
    };

    $scope.selectstate = function (c, addr) {
        addr.stateSearch = c.name;
        addr.Txtstate = c.Id;
        addr.showstateDropdown = false;
        addr.TxtCountry = c.countryid;

        try {
            var curid = parseInt(addr.TxtCountry);
            var result = $scope.CountryList.filter(function (x) {
                return x.Id == curid;
            });

            if (result.length > 0) {
                addr.CountrySearch = result[0].name;
            }

            if ($scope.newCustomer.QType == 'update') {
                $scope.saveCustomeraddressauto(addr);
            }
        } catch (error) {
            console.error("State select error:", error.message);
        }
    };

    // Delete Address
    $scope.confirmDeleteaddress = function (id, index) {
        Swal.fire({
            title: "Delete?",
            text: "Are you sure you want to delete this address?",
            icon: "warning",
            showCancelButton: true,
            confirmButtonText: "Yes, Delete",
            cancelButtonText: "Cancel"
        }).then((result) => {
            if (result.isConfirmed) {
                $scope.deleteAddress(id, index);
            }
        });
    };

    $scope.deleteAddress = function (addressid, index) {
        debugger;

        if ($scope.newCustomer.QType == "update") {
            $http({
                method: "POST",
                url: "/customer/default.aspx/deleteCustomeraddress",
                headers: { "Content-Type": "application/json" },
                data: JSON.stringify({ id: addressid })
            })
                .then(function (response) {
                    debugger;
                    let result = response.data.d;

                    if (result === "success") {
                        $scope.showToast("✔ Address Deleted successfully!", 4000);
                    } else {
                        alert("Error: " + result);
                    }
                }).catch(function (error) {
                    debugger;
                    console.error("HTTP ERROR:", error);

                    let msg = "Unknown error";
                    if (error.data && error.data.Message) {
                        msg = error.data.Message;
                    } else if (error.statusText) {
                        msg = error.statusText;
                    }

                    alert("HTTP Error Occurred:\n\n" +
                        "Status: " + error.status + "\n" +
                        "Message: " + msg);
                })
                .finally(() => {
                    debugger;
                    $scope.isSaving = false;
                });
        }

        debugger;

        $timeout(function () {
            $scope.newCustomer.addresses.splice(index, 1);
        });
    };

    // New customer
    $scope.newCustomer = {
        Id: 0,
        Regid: null,
        ContactType: 1,
        Cdate: null,
        Uid: null,
        Txtsalutation: "",
        Txtfirstname: "",
        Txtlastname: "",
        Txtcompanyname: "",
        Txtcontactdisplayname: "",
        TxtContactEmail: "",
        TxtworkphonePhone: "",
        Txtmobile: "",
        TxtSkypeName: "",
        TxtDesignation: "",
        TxtDepartment: "",
        Website: "",
        VATTreatment: "",
       // VATTreatmenttext: ($scope.countryid == 1) ? "VAT Treatment" : "GST Treatment",
        TaxRegistrationNumber: "",
        PlaceOfSupply: "",
        TxtCurrency: "0",
        PaymentTerms: "",
        IsEnablePortal: null,
        PortalLanguage: null,
        addresses: [],
        Facebook: "",
        Twitter: "",
        txtcreditlimit: null,
        txtcreditperiod: null,
        Rdltcs: null,
        Cuspricetype: null,
        Openingval: null,
        panno: "",
        Tanno: "",
        istds: null,
        sectionid: null,
        Salepersonid: null,
        PaymentTermsdisplay: "",
        Msale: null,
        Txtbankname: "",
        Txtaccountno: "",
        Txtifccode: "",
        Txtcorporateid: "",
        Txtbranch: "",
        Txtbranchaddress: "",
        Txtbenificiaryemail: "",
        txtcinno: "",
        Txtmodeofpayment: "",
        Txtfreightterms: "",
        Txtdeliverytype: "",
        Txttransport: "",
        TxtSplInstructoin: "",
        Txtsalevalueprvyear: "",
        Vcode: "",
        PartyType: null,
        Isdiscontinue: null,
        Tdspercent: null,
        TxtGraceLimit: null,
        TxtGracePeriod: null,
        ParentCompany: null,
        CreationDate: null,
        Customertype: "",
        ContactPersonDetails: [],
        ContactBankDetails: [],
        customerphoto: ""
    };

    $scope.defaultCustomer = {
        Id: 0,
        Regid: null,
        ContactType: 1,
        Cdate: null,
        Uid: null,
        Txtsalutation: "",
        Txtfirstname: "",
        Txtlastname: "",
        Txtcompanyname: "",
        Txtcontactdisplayname: "",
        TxtContactEmail: "",
        TxtworkphonePhone: "",
        Txtmobile: "",
        TxtSkypeName: "",
        TxtDesignation: "",
        TxtDepartment: "",
        Website: "",
        VATTreatment: "",
        //VATTreatmenttext:"VAT Treatment",
        TaxRegistrationNumber: "",
        PlaceOfSupply: "",
        TxtCurrency: "0",
        PaymentTerms: "",
        IsEnablePortal: null,
        PortalLanguage: null,
        addresses: [],
        Facebook: "",
        Twitter: "",
        txtcreditlimit: null,
        txtcreditperiod: null,
        Rdltcs: null,
        Cuspricetype: null,
        Openingval: null,
        panno: "",
        Tanno: "",
        istds: null,
        Sectionid: null,
        Salepersonid: null,
        PaymentTermsdisplay: "",
        Msale: null,
        Txtbankname: "",
        Txtaccountno: "",
        Txtifccode: "",
        Txtcorporateid: "",
        Txtbranch: "",
        Txtbranchaddress: "",
        Txtbenificiaryemail: "",
        txtcinno: "",
        Txtmodeofpayment: "",
        Txtfreightterms: "",
        Txtdeliverytype: "",
        Txttransport: "",
        TxtSplInstructoin: "",
        Txtsalevalueprvyear: "",
        Vcode: "",
        PartyType: null,
        Isdiscontinue: null,
        Tdspercent: null,
        TxtGraceLimit: null,
        TxtGracePeriod: null,
        ParentCompany: null,
        CreationDate: null,
        Customertype: "",
        ContactPersonDetails: [],
        ContactBankDetails: [],
        customerphoto: ""
    };
    if ($scope.newCustomer.istds == null) {
        $scope.newCustomer.istds = 0;
    }
    // newAddress
    $scope.newAddress = {
        Billing: true,
        Shipping: false,
        LocationID: "",
        street1: "",
        street2: "",
        city: "",
        stateSearch: "",
        Txtstate: "",
        TxtCountry: "",
        CountrySearch: "",
        zipcode: "",
        id: 0
    };

    $scope.addAddress = function (form) {
        if (form.$invalid) {
            angular.forEach(form.$error, function (fields) {
                angular.forEach(fields, function (field) {
                    field.$setTouched();
                });
            });
            return;
        }

        if ($scope.newCustomer.addresses == null) {
            $scope.newCustomer.addresses = [];
        }

        if ($scope.newCustomer.QType == "update") {
            $http({
                method: "POST",
                url: "/customer/default.aspx/AddCustomeraddress",
                headers: { "Content-Type": "application/json" },
                data: JSON.stringify({ cusid: $scope.newCustomer.Id, addr: $scope.newAddress })
            })
                .then(function (response) {
                    debugger;
                    let result = response.data.d;

                    if (result === "success") {
                        $scope.showToast("✔ Address saved successfully!", 4000);
                    } else {
                        alert("Error: " + result);
                    }
                }).catch(function (error) {
                    debugger;
                    console.error("HTTP ERROR:", error);

                    let msg = "Unknown error";
                    if (error.data && error.data.Message) {
                        msg = error.data.Message;
                    } else if (error.statusText) {
                        msg = error.statusText;
                    }

                    alert("HTTP Error Occurred:\n\n" +
                        "Status: " + error.status + "\n" +
                        "Message: " + msg);
                })
                .finally(() => {
                    debugger;
                    $scope.isSaving = false;
                });
        }

        $scope.newCustomer.addresses.push(angular.copy($scope.newAddress));

        $scope.newAddress = {
            Billing: true,
            Shipping: false,
            LocationID: "",
            street1: "",
            street2: "",
            city: "",
            stateSearch: "",
            Txtstate: "",
            TxtCountry: "",
            CountrySearch: "",
            zipcode: "",
            id: 0
        };

        $timeout(function () {
            if (typeof initSearchDropdown === "function") {
                initSearchDropdown();
            }
        }, 0);
    };

    $scope.saveCustomeraddressauto = function (newAddress) {
        if ($scope.newCustomer.QType == "update") {
            $http({
                method: "POST",
                url: "/customer/default.aspx/updateCustomeraddress",
                headers: { "Content-Type": "application/json" },
                data: JSON.stringify({ cusid: $scope.newCustomer.Id, addr: newAddress })
            })
                .then(function (response) {
                    debugger;
                    let result = response.data.d;

                    if (result !== "success") {
                        alert("Error: " + result);
                    }
                }).catch(function (error) {
                    debugger;
                    console.error("HTTP ERROR:", error);

                    let msg = "Unknown error";
                    if (error.data && error.data.Message) {
                        msg = error.data.Message;
                    } else if (error.statusText) {
                        msg = error.statusText;
                    }

                    alert("HTTP Error Occurred:\n\n" +
                        "Status: " + error.status + "\n" +
                        "Message: " + msg);
                })
                .finally(() => {
                    debugger;
                    $scope.isSaving = false;
                });
        }
    };

    $scope.removeAddress = function (index) {
        $scope.newCustomer.addresses.splice(index, 1);
    };

    // NewContactPerson
    $scope.newContactPerson = {
        Id: 0,
        Salutation: "",
        FirstName: "",
        LastName: "",
        EmailAddress: "",
        WorkPhone: "",
        Mobile: "",
        SkypeName: "",
        Designation: "",
        Department: "",
        notes: "",
        DepartmentSearch: "",
        customerphoto: ""
    };

    $scope.addcontact = function () {
        if ($scope.newCustomer.ContactPersonDetails == null) {
            $scope.newCustomer.ContactPersonDetails = [];
        }

        if ($scope.newCustomer.QType == "update") {
            $http({
                method: "POST",
                url: "/customer/default.aspx/AddCustomercontact",
                headers: { "Content-Type": "application/json" },
                data: JSON.stringify({ cusid: $scope.newCustomer.Id, addr: $scope.newContactPerson })
            })
                .then(function (response) {
                    debugger;
                    let result = response.data.d;

                    if (result === "success") {
                        $scope.showToast("✔ Contact saved successfully!", 4000);
                    } else {
                        alert("Error: " + result);
                    }
                }).catch(function (error) {
                    debugger;
                    console.error("HTTP ERROR:", error);

                    let msg = "Unknown error";
                    if (error.data && error.data.Message) {
                        msg = error.data.Message;
                    } else if (error.statusText) {
                        msg = error.statusText;
                    }

                    alert("HTTP Error Occurred:\n\n" +
                        "Status: " + error.status + "\n" +
                        "Message: " + msg);
                })
                .finally(() => {
                    debugger;
                    $scope.isSaving = false;
                });
        }

        $scope.newCustomer.ContactPersonDetails.push(angular.copy($scope.newContactPerson));

        $scope.newContactPerson = {
            Id: 0,
            Salutation: "",
            FirstName: "",
            LastName: "",
            EmailAddress: "",
            WorkPhone: "",
            Mobile: "",
            SkypeName: "",
            Designation: "",
            Department: "",
            notes: "",
            DepartmentSearch: ""
        };

        $timeout(function () {
            if (typeof initSearchDropdown === "function") {
                initSearchDropdown();
            }
        }, 0);
    };

    $scope.saveCustomercontactauto = function (ContactPersonDetails) {
        if ($scope.newCustomer.QType == "update") {
            $http({
                method: "POST",
                url: "/customer/default.aspx/updateCustomercontact",
                headers: { "Content-Type": "application/json" },
                data: JSON.stringify({ cusid: $scope.newCustomer.Id, addr: ContactPersonDetails })
            })
                .then(function (response) {
                    debugger;
                    let result = response.data.d;

                    if (result !== "success") {
                        alert("Error: " + result);
                    }
                }).catch(function (error) {
                    debugger;
                    console.error("HTTP ERROR:", error);

                    let msg = "Unknown error";
                    if (error.data && error.data.Message) {
                        msg = error.data.Message;
                    } else if (error.statusText) {
                        msg = error.statusText;
                    }

                    alert("HTTP Error Occurred:\n\n" +
                        "Status: " + error.status + "\n" +
                        "Message: " + msg);
                })
                .finally(() => {
                    debugger;
                    $scope.isSaving = false;
                });
        }
    };

    $scope.confirmDeleteContactPersonDetails = function (id, index) {
        Swal.fire({
            title: "Delete?",
            text: "Are you sure you want to delete this Contact?",
            icon: "warning",
            showCancelButton: true,
            confirmButtonText: "Yes, Delete",
            cancelButtonText: "Cancel"
        }).then((result) => {
            if (result.isConfirmed) {
                $scope.DeleteContactPersonDetails(id, index);
            }
        });
    };

    $scope.DeleteContactPersonDetails = function (addressid, index) {
        debugger;
        if ($scope.newCustomer.QType == "update") {
            $http({
                method: "POST",
                url: "/customer/default.aspx/deleteCustomercontact",
                headers: { "Content-Type": "application/json" },
                data: JSON.stringify({ id: addressid })
            })
                .then(function (response) {
                    debugger;
                    let result = response.data.d;

                    if (result === "success") {
                        $scope.showToast("✔ Contact Deleted successfully!", 4000);
                    } else {
                        alert("Error: " + result);
                    }
                }).catch(function (error) {
                    debugger;
                    console.error("HTTP ERROR:", error);

                    let msg = "Unknown error";
                    if (error.data && error.data.Message) {
                        msg = error.data.Message;
                    } else if (error.statusText) {
                        msg = error.statusText;
                    }

                    alert("HTTP Error Occurred:\n\n" +
                        "Status: " + error.status + "\n" +
                        "Message: " + msg);
                })
                .finally(() => {
                    debugger;
                    $scope.isSaving = false;
                });
        }

        debugger;

        $timeout(function () {
            $scope.newCustomer.ContactPersonDetails.splice(index, 1);
        });
    };

    $scope.getInitials = function (name) {
        if (!name) return "";

        var words = name.trim().split(" ");
        var initials = "";

        angular.forEach(words, function (word) {
            if (word.length > 0) {
                initials += word.charAt(0).toUpperCase();
            }
        });

        return initials;
    };

    // Department list
    $scope.departmentList = [];

    $scope.loaddepartmentList = function () {
        $http({
            method: "POST",
            url: "/customer/default.aspx/getdepartmentlist",
            headers: { "Content-Type": "application/json" },
            data: JSON.stringify({ regid: "1" })
        }).then(function (response) {
            $scope.departmentList = response.data.d;
        }).catch(function (error) {
            debugger;
            console.error("HTTP ERROR:", error);
        });
    };

    $scope.loaddepartmentList();

    $scope.DepartmentSearch = "";
    $scope.TxtDepartment = "";
    $scope.showDepartmentDropdown = false;
    $scope.departmentHighlightIndex = -1;

    $scope.filterDepartment = function (item) {
        if (!$scope.newContactPerson.DepartmentSearch) return true;
        let q = $scope.newContactPerson.DepartmentSearch.toLowerCase();
        return (item.name).toLowerCase().includes(q);
    };

    $scope.selectDepartmentContactPerson = function (c) {
        $scope.newContactPerson.TxtDepartment = c.Id;
        $scope.newContactPerson.DepartmentSearch = c.name;
        $scope.showDepartmentDropdown = false;
        $scope.departmentHighlightIndex = -1;
    };

    $scope.selectDepartment = function (c, addr) {
        addr.TxtDepartment = c.Id;
        addr.DepartmentSearch = c.name;
        addr.showDepartmentDropdown = false;
        addr.highlightIndex = -1;

        if ($scope.newCustomer.QType == 'update') {
            $scope.saveCustomercontactauto(addr);
        }
    };

    // New Bank details
    $scope.newbankdetail = {
        id: 0,
        BankName: "",
        AccountNumber: "",
        IFSCCode: "",
        IBANCode: "",
        LocationBranch: "",
        BeneficiaryEmail: "",
        BranchAddress: ""
    };

    $scope.addbankdetail = function () {
        if ($scope.newCustomer.ContactBankDetails == null) {
            $scope.newCustomer.ContactBankDetails = [];
            
        }

        if ($scope.newCustomer.QType == "update") {
            $http({
                method: "POST",
                url: "/customer/default.aspx/AddCustomerbankdetails",
                headers: { "Content-Type": "application/json" },
                data: JSON.stringify({ cusid: $scope.newCustomer.Id, addr: $scope.newbankdetail })
            })
                .then(function (response) {
                    debugger;
                    let result = response.data.d;

                    if (result === "success") {
                        $scope.showToast("✔ Bank details saved successfully!", 4000);
                    } else {
                        alert("Error: " + result);
                    }
                }).catch(function (error) {
                    debugger;
                    console.error("HTTP ERROR:", error);

                    let msg = "Unknown error";
                    if (error.data && error.data.Message) {
                        msg = error.data.Message;
                    } else if (error.statusText) {
                        msg = error.statusText;
                    }

                    alert("HTTP Error Occurred:\n\n" +
                        "Status: " + error.status + "\n" +
                        "Message: " + msg);
                })
                .finally(() => {
                    debugger;
                    $scope.isSaving = false;
                });
        }

        $scope.newCustomer.ContactBankDetails.push(angular.copy($scope.newbankdetail));

        $scope.newbankdetail = {
            id: 0,
            BankName: "",
            AccountNumber: "",
            IFSCCode: "",
            IBANCode: "",
            LocationBranch: "",
            BeneficiaryEmail: "",
            BranchAddress: ""
        };

        $timeout(function () {
            if (typeof initSearchDropdown === "function") {
                initSearchDropdown();
            }
        }, 0);
    };

    $scope.saveCustomerbankdetailsauto = function (ContactBankDetails) {
        if ($scope.newCustomer.QType == "update") {
            $http({
                method: "POST",
                url: "/customer/default.aspx/updateCustomerbankdetails",
                headers: { "Content-Type": "application/json" },
                data: JSON.stringify({ cusid: $scope.newCustomer.Id, addr: ContactBankDetails })
            })
                .then(function (response) {
                    debugger;
                    let result = response.data.d;

                    if (result !== "success") {
                        alert("Error: " + result);
                    }
                }).catch(function (error) {
                    debugger;
                    console.error("HTTP ERROR:", error);

                    let msg = "Unknown error";
                    if (error.data && error.data.Message) {
                        msg = error.data.Message;
                    } else if (error.statusText) {
                        msg = error.statusText;
                    }

                    alert("HTTP Error Occurred:\n\n" +
                        "Status: " + error.status + "\n" +
                        "Message: " + msg);
                })
                .finally(() => {
                    debugger;
                    $scope.isSaving = false;
                });
        }
    };

    $scope.confirmDeletebankdetail = function (id, index) {
        Swal.fire({
            title: "Delete?",
            text: "Are you sure you want to delete this Bank Details?",
            icon: "warning",
            showCancelButton: true,
            confirmButtonText: "Yes, Delete",
            cancelButtonText: "Cancel"
        }).then((result) => {
            if (result.isConfirmed) {
                $scope.Deletebankdetail(id, index);
            }
        });
    };

    $scope.Deletebankdetail = function (addressid, index) {
        debugger;
        if ($scope.newCustomer.QType == "update") {
            $http({
                method: "POST",
                url: "/customer/default.aspx/deletebankdetails",
                headers: { "Content-Type": "application/json" },
                data: JSON.stringify({ id: addressid })
            })
                .then(function (response) {
                    debugger;
                    let result = response.data.d;

                    if (result === "success") {
                        $scope.showToast("✔ Bank Dteils Deleted successfully!", 4000);
                    } else {
                        alert("Error: " + result);
                    }
                }).catch(function (error) {
                    debugger;
                    console.error("HTTP ERROR:", error);

                    let msg = "Unknown error";
                    if (error.data && error.data.Message) {
                        msg = error.data.Message;
                    } else if (error.statusText) {
                        msg = error.statusText;
                    }

                    alert("HTTP Error Occurred:\n\n" +
                        "Status: " + error.status + "\n" +
                        "Message: " + msg);
                })
                .finally(() => {
                    debugger;
                    $scope.isSaving = false;
                });
        }

        debugger;

        $timeout(function () {
            $scope.newCustomer.ContactBankDetails.splice(index, 1);
        });
    };

    // Save customer
    $scope.saveCustomer = function () {
        debugger;
        
        if ($scope.newCustomer.Txtcontactdisplayname != '') {
            $scope.isSaving = true;
            $scope.newCustomer.ContactType = 1;
            $scope.newCustomer.Txtfirstname = "";
            $scope.newCustomer.Txtlastname = "";
            $scope.newCustomer.Txtcompanyname = $scope.newCustomer.Txtcontactdisplayname;
            $http({
                method: "POST",
                url: "/customer/default.aspx/AddCustomer",
                headers: { "Content-Type": "application/json" },
                data: JSON.stringify({ model: $scope.newCustomer })
            })
                .then(function (response) {
                    debugger;
                    let result = response.data.d;

                    if (result === "success") {
                        $scope.showToast("✔ Customer saved successfully!", 4000);
                        $scope.showlist = true;
                        $scope.newCustomer = angular.copy($scope.defaultCustomer);
                        $scope.newCustomer.QType = "insert";
                        $scope.showAddForm = false;
                        $scope.loadCustomers();
                    } else {
                        alert("Error: " + result);
                    }
                }).catch(function (error) {
                    debugger;
                    console.error("HTTP ERROR:", error);

                    let msg = "Unknown error";
                    if (error.data && error.data.Message) {
                        msg = error.data.Message;
                    } else if (error.statusText) {
                        msg = error.statusText;
                    }

                    alert("HTTP Error Occurred:\n\n" +
                        "Status: " + error.status + "\n" +
                        "Message: " + msg);
                })
                .finally(() => {
                    debugger;
                    $scope.isSaving = false;
                });
        }
        else {
            $scope.showToasterror("Please Enter Customer Name!", 4000);
            document.getElementById("txtcustomername").focus();
          
        }
    };

    // Auto save customer
    $scope.saveCustomerauto = function () {
        debugger;

        if ($scope.newCustomer.QType == "update") {
            $scope.newCustomer.Txtcompanyname = $scope.newCustomer.Txtcontactdisplayname;
            $http({
                method: "POST",
                url: "/customer/default.aspx/AddCustomer",
                headers: { "Content-Type": "application/json" },
                data: JSON.stringify({ model: $scope.newCustomer })
            })
                .then(function (response) {
                    debugger;
                    let result = response.data.d;

                    if (result === "success") {
                        $scope.loadCustomers();
                    } else {
                        alert("Error: " + result);
                    }
                }).catch(function (error) {
                    debugger;
                    console.error("HTTP ERROR:", error);

                    let msg = "Unknown error";
                    if (error.data && error.data.Message) {
                        msg = error.data.Message;
                    } else if (error.statusText) {
                        msg = error.statusText;
                    }

                    alert("HTTP Error Occurred:\n\n" +
                        "Status: " + error.status + "\n" +
                        "Message: " + msg);
                });
        }
    };

    $scope.showToast = function (msg, duration = 3000) {
        let toast = document.getElementById("toast");
        let msgBox = document.getElementById("toastMessage");
        let bar = document.getElementById("toastBar");

        if (!toast || !msgBox || !bar) return;

        msgBox.innerHTML = msg;

        bar.style.transition = "none";
        bar.style.width = "100%";

        toast.classList.add("show");

        setTimeout(() => {
            bar.style.transition = `width ${duration}ms linear`;
            bar.style.width = "0%";
        }, 50);

        setTimeout(() => {
            toast.classList.remove("show");
        }, duration + 200);
    };
    $scope.showToasterror = function (msg, duration = 3000) {
        let toast = document.getElementById("toasterror");
        let msgBox = document.getElementById("toastMessageerror");
        let bar = document.getElementById("toastBarerror");

        if (!toast || !msgBox || !bar) return;

        msgBox.innerHTML = msg;

        bar.style.transition = "none";
        bar.style.width = "100%";

        toast.classList.add("show");

        setTimeout(() => {
            bar.style.transition = `width ${duration}ms linear`;
            bar.style.width = "0%";
        }, 50);

        setTimeout(() => {
            toast.classList.remove("show");
        }, duration + 200);
    };

    $scope.editCustomer = function (id) {
        $scope.showlist = false;
        $scope.newCustomer = angular.copy($scope.defaultCustomer);
        $scope.showAddForm = true;
        setCustomHash("mode=edit&id=" + id);
        $scope.loadCustomerbyid(id);
    };

    $scope.loadCustomerbyid = function (id) {
        var el = document.getElementById("show-billing-address");
        if (el) {
            el.classList.remove("show");
        }

        debugger;

        $http({
            method: "POST",
            url: "/customer/default.aspx/GetCustomersbyid",
            headers: { "Content-Type": "application/json" },
            data: JSON.stringify({ CustomerID: id || "0" })
        })
            .then(function (response) {
                $scope.newCustomer = response.data.d;
                $scope.newCustomer.QType = "update";
                $scope.newCustomer.Id = id;

                try {
                    var curid = parseInt($scope.newCustomer.TxtCurrency);
                    var result = $scope.currencyList.filter(function (x) {
                        return x.Id == curid;
                    });

                    if (result.length > 0) {
                        debugger;
                        $scope.newCustomer.currencySearch = result[0].name;
                    }
                } catch (error) {
                    debugger;
                    console.error("Currency select error:", error.message);
                }

                try {
                    debugger;
                    var parentid = parseInt($scope.newCustomer.ParentCompany);
                    var parentResult = $scope.ParentsList.filter(function (x) {
                        return x.Id == parentid;
                    });

                    if (parentResult.length > 0) {
                        $scope.newCustomer.TxtParent = parentResult[0].name;
                    }
                } catch (error) {
                    console.error("Parent select error:", error.message);
                }

                try {
                    if ($scope.newCustomer.customerphoto == "") {
                        $scope.newCustomer.customerphoto = "/angularmaster/assets/images/company.png";
                    }
                } catch (error) {
                    console.error("Photo load error:", error.message);
                }

                try {
                    var industryid = parseInt($scope.newCustomer.IndustryCompany);
                    var industryResult = $scope.IndustrysList.filter(function (x) {
                        return x.Id == industryid;
                    });

                    if (industryResult.length > 0) {
                        $scope.newCustomer.TxtIndustry = industryResult[0].name;
                    }
                } catch (error) {
                    debugger;
                    console.error("Industry select error:", error.message);
                }

                try {
                    var spidid = parseInt($scope.newCustomer.Salepersonid);
                    var spresult = $scope.salespersonList.filter(function (x) {
                        return x.Id == spidid;
                    });

                    if (spresult.length > 0) {
                        debugger;
                        $scope.newCustomer.salespersonSearch = spresult[0].name;
                    }
                } catch (error) {
                    debugger;
                    console.error("Currency select error:", error.message);
                }

                try {
                    var VATTreatmentid = parseInt($scope.newCustomer.VATTreatment);
                    var VATTreatmentresult = $scope.VATTreatmentList.filter(function (x) {
                        return x.Id == VATTreatmentid;
                    });

                    if (VATTreatmentresult.length > 0) {
                        debugger;
                        $scope.newCustomer.TxtVATTreatment = VATTreatmentresult[0].name;
                    }
                } catch (error) {
                    debugger;
                    console.error("Currency select error:", error.message);
                }



                try {
                    debugger;
                    var accountreceivableid = parseInt($scope.newCustomer.accountreceivable);
                    //alert(accountreceivableid);

                    var accountreceivableResult = $scope.AccountLedgerList.filter(function (x) {
                        return x.Id == accountreceivableid;
                    });

                    if (accountreceivableResult.length > 0) {
                        $scope.newCustomer.Txtaccountreceivable = accountreceivableResult[0].groupname;
                    }
                } catch (error) {
                    console.error("Parent select error:", error.message);
                }

                try {
                    debugger;
                    var accountpayableid = parseInt($scope.newCustomer.accountpayable);
                    var accountpayableResult = $scope.AccountLedgerList.filter(function (x) {
                        return x.Id == accountpayableid;
                    });

                    if (accountpayableResult.length > 0) {
                        $scope.newCustomer.Txtaccountpayable = accountpayableResult[0].groupname;
                    }
                } catch (error) {
                    console.error("Parent select error:", error.message);
                }

                try {
                    debugger;
                    var sectionidvalue = parseInt($scope.newCustomer.sectionid);
                    var sectionidResult = $scope.sectionList.filter(function (x) {
                        return x.Id == sectionidvalue;
                    });

                    if (sectionidResult.length > 0) {
                        $scope.newCustomer.Txtsectionid = sectionidResult[0].name;
                    }
                } catch (error) {
                    console.error("Parent select error:", error.message);
                }


                $timeout(function () {
                    if (typeof initSearchDropdown === "function") {
                        initSearchDropdown();
                    }
                }, 0);
            })
            .catch(function (error) {
                debugger;
                console.error("HTTP Error:", error);
            });
    };



    ///Upload image customer

    $scope.readURL = function (input) {
        if (input.files && input.files[0]) {
            var reader = new FileReader();

            reader.onload = function (e) {
                $scope.$apply(function () {
                    $scope.newCustomer.customerphoto = e.target.result;
                });
            };

            reader.readAsDataURL(input.files[0]);
            $scope.newCustomer.customerphoto = input.files[0];
            $scope.uploadImage();
        }
    };

    $scope.uploadDrop = function (event) {
        event.preventDefault();

        var file = event.dataTransfer.files[0];
        if (!file) return;

        var reader = new FileReader();

        reader.onload = function (e) {
            $scope.$apply(function () {
                $scope.newCustomer.customerphoto = e.target.result;
            });
        };

        reader.readAsDataURL(file);
        $scope.newCustomer.customerphoto = file;
        $scope.uploadImage();
    };

    $scope.uploadImage = function () {
        debugger;
        var formData = new FormData();
        formData.append("file", $scope.newCustomer.customerphoto);

        $http({
            method: "POST",
            url: "/customer/customerUploadHandler.ashx",
            data: formData,
            transformRequest: angular.identity,
            headers: { 'Content-Type': undefined }
        }).then(function (response) {
            if (typeof response.data === "string" && response.data.indexOf("Error:") === 0) {
                alert(response.data);
                return;
            }

            $scope.newCustomer.customerphoto = response.data;

            if ($scope.newCustomer.QType == "update") {
                $scope.saveCustomerauto();
            }
        }, function () {
            alert("Upload Failed");
        });
    };
    //End upload image customer



    ///Upload image customer contact

    $scope.readURLcontact = function (input) {
        alert(input.files);
        if (input.files && input.files[0]) {
            var reader = new FileReader();

            reader.onload = function (e) {
                $scope.$apply(function () {
                    $scope.newContactPerson.customerphoto = e.target.result;

                    alert($scope.newContactPerson.customerphoto);
                });
            };

            reader.readAsDataURL(input.files[0]);
            $scope.newContactPerson.customerphoto = input.files[0];
            $scope.uploadImagecontact();
        }
    };

    $scope.uploadDropcontact = function (event) {
        event.preventDefault();

        var file = event.dataTransfer.files[0];
        if (!file) return;

        var reader = new FileReader();

        reader.onload = function (e) {
            $scope.$apply(function () {
                $scope.newContactPerson.customerphoto = e.target.result;
            });
        };

        reader.readAsDataURL(file);
        $scope.newContactPerson.customerphoto = file;
        $scope.uploadImagecontact();
    };

    $scope.uploadImagecontact = function () {
        debugger;
        var formData = new FormData();
        formData.append("file", $scope.newContactPerson.customerphoto);

        $http({
            method: "POST",
            url: "/customer/customerUploadHandlercontact.ashx",
            data: formData,
            transformRequest: angular.identity,
            headers: { 'Content-Type': undefined }
        }).then(function (response) {
            if (typeof response.data === "string" && response.data.indexOf("Error:") === 0) {
                alert(response.data);
                return;
            }

            $scope.newContactPerson.customerphoto = response.data;

            if ($scope.newCustomer.QType == "update") {
                //$scope.saveCustomerauto();
            }
        }, function () {
            alert("Upload Failed");
        });
    };
    //End upload image customer contact



    ///Upload image customer contact edit

    $scope.readURLcontactgrid = function (index, addr, event) {

        var input = event.target;

        if (input.files && input.files[0]) {

            var file = input.files[0];
            var reader = new FileReader();

            reader.onload = function (e) {
                $scope.$apply(function () {
                    addr.customerphoto = e.target.result;
                });
            };

            reader.readAsDataURL(file);

            addr.customerphoto = file;

            $scope.uploadImagecontactgrid(addr);
        }
    };

    $scope.uploadDropcontactgrid = function (event, addr) {
        event.preventDefault();

        var file = null;

        if (event.originalEvent && event.originalEvent.dataTransfer) {
            file = event.originalEvent.dataTransfer.files[0];
        } else if (event.dataTransfer) {
            file = event.dataTransfer.files[0];
        }

        if (!file) return;

        var reader = new FileReader();

        reader.onload = function (e) {
            $scope.$apply(function () {
                addr.customerphoto = e.target.result;
            });
        };

        reader.readAsDataURL(file);

        addr.customerphoto = file;
        $scope.uploadImagecontactgrid(addr);
    };

    $scope.uploadImagecontactgrid = function (addr) {
        debugger;
        var formData = new FormData();
        formData.append("file", addr.customerphoto);

        $http({
            method: "POST",
            url: "/customer/customerUploadHandlercontact.ashx",
            data: formData,
            transformRequest: angular.identity,
            headers: { 'Content-Type': undefined }
        }).then(function (response) {
            if (typeof response.data === "string" && response.data.indexOf("Error:") === 0) {
                alert(response.data);
                return;
            }

            addr.customerphoto = response.data;

            if ($scope.newCustomer.QType == "update") {
                $scope.saveCustomercontactauto(addr);
                //$scope.saveCustomerauto();
            }
        }, function () {
            alert("Upload Failed");
        });
    };
    //End upload image customer contact edit


    // Download Excel
    $scope.downloadExcel = function () {
        var ws = XLSX.utils.json_to_sheet($scope.customers);
        var wb = XLSX.utils.book_new();
        XLSX.utils.book_append_sheet(wb, ws, "Customers");
        XLSX.writeFile(wb, "Customers.xlsx");
    };

    // Download PDF
    $scope.downloadPDF = function () {
        const { jsPDF } = window.jspdf;
        const doc = new jsPDF();

        doc.autoTable({
            head: [['Sr No', 'Name', 'Email', 'Mobile', 'Customer Code', 'GST Number', 'Primary Contact person', 'City']],
            body: $scope.customers.map((c, i) => [
                i + 1,
                c.Txtcontactdisplayname,
                c.TxtContactEmail,
                c.Txtmobile,
                c.Vcode,
                c.TaxRegistrationNumber,
                c.Txtfirstname,
                c.BillCity
            ])
        });

        doc.save("Customers.pdf");
    };

    $scope.showlist = true;
    $scope.showAddForm = false;
    $scope.newCustomer = angular.copy($scope.defaultCustomer);

    $scope.cancelAdd = function () {
        $scope.newCustomer = angular.copy($scope.defaultCustomer);
        $scope.showAddForm = false;
        $scope.showlist = true;
    };

    $scope.openAddCustomer = function (mode) {
        if ($scope.newCustomer.QType == "update") {
            if (confirm("Are you sure you want to Leave This Page?")) {
                $scope.showlist = false;
                $scope.newCustomer = angular.copy($scope.defaultCustomer);
                $scope.newCustomer.QType = "insert";
                $scope.showAddForm = true;
                setCustomHash("mode=" + mode);
            }
        }
        else {
            $scope.showlist = false;
            $scope.newCustomer = angular.copy($scope.defaultCustomer);
            $scope.newCustomer.QType = "insert";
            $scope.showAddForm = true;
            setCustomHash("mode=" + mode);
        }
    };

    $scope.saveNewCustomer = function () {
        $http({
            method: "POST",
            url: "/customer/default.aspx/AddCustomer",
            headers: { "Content-Type": "application/json" },
            data: JSON.stringify({ model: $scope.newCustomer })
        }).then(function (response) {
            let added = response.data.d;
            $scope.customers.push(added);
            $scope.showAddForm = false;
            alert("Customer added successfully!");
            $scope.showlist = true;
        });
    };



    $scope.customerChangeLog = [];
    $scope._customerSnapshot = angular.copy($scope.newCustomer);

    function addOrUpdateLog(path, oldValue, newValue) {
        if (angular.equals(oldValue, newValue)) return;

        var existing = $scope.customerChangeLog.find(function (x) {
            return x.field === path;
        });

        if (existing) {
            existing.newValue = angular.copy(newValue);
            existing.changedAt = new Date();
        } else {
            $scope.customerChangeLog.push({
                field: path,
                oldValue: angular.copy(oldValue),
                newValue: angular.copy(newValue),
                changedAt: new Date(),
                customerId: $scope.newCustomer.Id || 0
            });
        }
    }

    function compareObjects(oldObj, newObj, parentPath) {
        oldObj = oldObj || {};
        newObj = newObj || {};

        var allKeys = {};
        angular.forEach(oldObj, function (v, k) { allKeys[k] = true; });
        angular.forEach(newObj, function (v, k) { allKeys[k] = true; });

        angular.forEach(allKeys, function (v, key) {
            var path = parentPath ? parentPath + "." + key : key;
            var oldVal = oldObj[key];
            var newVal = newObj[key];

            if (angular.isObject(oldVal) && angular.isObject(newVal) &&
                !angular.isArray(oldVal) && !angular.isArray(newVal)) {
                compareObjects(oldVal, newVal, path);
            } else if (!angular.equals(oldVal, newVal)) {
                addOrUpdateLog(path, oldVal, newVal);
            }
        });
    }

    $scope.$watch('newCustomer', function (newVal) {
        if (!newVal) return;
        compareObjects($scope._customerSnapshot, newVal, "");
        $scope._customerSnapshot = angular.copy(newVal);
    }, true);


});