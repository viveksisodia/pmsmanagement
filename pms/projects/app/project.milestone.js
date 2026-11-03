
app.controller("projectmilestoneCtrl", function ($scope, projectService, projectState) {
    $scope.projectState = projectState;
    $scope.openAdd = function (mode) { 
        if ($scope.newitem.QType == "update") {
            if (confirm("Are you sure you want to Leave This Page?")) {
                $scope.projectState.showlist = false;
                $scope.newitem = angular.copy($scope.defaultnewitem);
                $scope.newitem.QType = "insert";
                $scope.projectState.showAddForm = true;
                setCustomHash("mode=" + mode);
            }
        }
        else {
            initSearchDropdown();
            $scope.projectState.showlist = false;
            $scope.newitem = angular.copy($scope.defaultnewitem);
            $scope.newitem.QType = "insert";
            $scope.projectState.showAddForm = true;
            setCustomHash("mode=" + mode);
        }
    };

    $scope.cancelAdd = function () {
        $scope.newitem = angular.copy($scope.defaultnewitem);
        $scope.projectState.showAddForm = false;
        $scope.projectState.showlist = true;
        window.location.hash = "";
    };
    function setCustomHash(value) {
        window.location.hash = "##" + value;
    } 
    $scope.$on("editProductEvent", function (event, id) {
       // alert("Child controller received ID: " + id);
        // your edit logic here
        $scope.editproduct(id);
    });



    $scope.editproduct = function (id) {
        $scope.projectState.showlist = false;
        $scope.newitem = angular.copy($scope.defaultnewitem);
        $scope.projectState.showAddForm = true;
        $scope.loadproductbyid(id);
        setCustomHash("mode=edit&id=" + id);
        
    };


    $scope.loadproductbyid = function (id) {       

    
        debugger;
        projectService.Getproductbyid(id)
            .then(function (response) {
                $scope.newitem = response.data.d;
                initSearchDropdown();
                $scope.newitem.ptype = String($scope.newitem.ptype);                
                $scope.newitem.ispurchase = String($scope.newitem.ispurchase);
                $scope.newitem.issale = String($scope.newitem.issale);
                $scope.newitem.isposenable = String($scope.newitem.isposenable);
                $scope.newitem.isRoll = String($scope.newitem.isRoll);
                $scope.newitem.isScrap = String($scope.newitem.isScrap);
                $scope.newitem.issalepurchasesame = String($scope.newitem.issalepurchasesame);
                $scope.newitem.QType = "update";
              


                try {
                    var categoryid = parseInt($scope.newitem.categoryid);
                    var result = $scope.CategoryList.filter(function (x) {
                        return x.Id == categoryid;
                    });

                    if (result.length > 0) {
                        debugger;
                        $scope.newitem.CategorySearch = result[0].name;
                        
                    }
                } catch (error) {
                    debugger;
                    console.error("Category select error:", error.message);
                }
                try {
                    var Taxid = parseInt($scope.newitem.salesTaxtype);                   
                    var resultTax = $scope.taxList.filter(function (x) {
                        return x.Id == Taxid;
                    });

                    if (resultTax.length > 0) {
                        debugger;
                        $scope.newitem.taxSearch = resultTax[0].name;

                    }
                } catch (error) {
                    debugger;
                    console.error("Tax select error:", error.message);
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


    $scope.newitem = [];
    $scope.defaultnewitem = {
        id: "0",
        ptype: "1",
        pname: "",
        Unitval: "",
        SKUval: "",
        exciseproduct: "0",
        issale: "1",
        isposenable:"0",
        salesRate: "",
        salesaccount: "",
        salesDescription: "",
        salesTaxtype: "",
        salestaxvalue: "",
        taxSearch:"",
        ispurchase: "1",
        PurchaseRate: "",
        Purchaseaccount: "",
        PurchaseDescription: "",
        PurchaseTaxtype: "0",
        Purchasetaxvalue: "0",
        isTrackInventory: "1",
        trackAccount: "",
        OpeningStock: "",
        OpeningStockrateperunit: "",
        pimg: "",
        categoryid: "",
        subcategoryid: "",
        subcategory2id: "",
        subcategory3id: "",
        subcategory4id: "",
        rsaleprice: "",
        rrentprice: "",
        rtype: "",
        Unitval2: "",
        unitrelationvalue: "",
        unitvalue: "",
        unitvalue2: "",
        issalepurchasesame:"0",
        barcode: "",
        txtreorder: "",
        istaxinclusive: "0",
        Wholesale_Price: "",
        Wholesale_istaxinclusive: "0",
        Franchise_Price: "",
        Franchise_istaxinclusive: "0",
        Discount: "",
        txtfocprice: "",
        ddlissemicomprehensive: "1",
        txtordercode: "",
        txtbrand: "",
        PcsCase_Bag: "",
        CaseBag_Size: "",
        groupId: "",
        txtsize: "",
        txtopensize: "",
        txtmicrion: "",
        txtspecificgravity: "",
        txtcutlength: "",
        MaxLimit: "",
        QType: "insert",
        CategorySearch: "",
        isRoll: "0",
        isScrap: "0",
        ddlinventorytype:"1"
         
    };
    $scope.newitem = angular.copy($scope.defaultnewitem);
   
    $scope.highlightMatch = function (text, search) {
        if (!search || search.trim() === "") return text;
        let pattern = new RegExp("(" + search + ")", "gi");
        return text.replace(pattern, "<b>$1</b>");
    };

    // tax list
    $scope.dropdownTyping=false;
    $scope.taxList = []; 
    $scope.taxSearch = "";
    $scope.showTaxDropdown = false;
    $scope.loadtaxList = function () {
        projectService.gettaxList("1").then(function (response) {
            $scope.taxList = response.data.d;
        }).catch(function (error) {
            debugger;
            alert("HTTP ERROR: " + (error.statusText || error.message || "Unknown error"));
            console.error("HTTP ERROR:", error);
        });
    };
    $scope.loadtaxList();
    $scope.selectTax = function (item) {
        $scope.newitem.taxSearch = item.name;
        $scope.newitem.salesTaxtype = item.id;
        $scope.newitem.salestaxvalue = item.taxvalue;
        $scope.showTaxDropdown = false;
    };
    $scope.filtertaxSearch = function (item) {
        return $scope.commonDropdownFilter(item, $scope.newitem.taxSearch, "name"); 
    };
    angular.element(document).on("click", function () {
        $scope.$applyAsync(function () {
            $scope.showTaxDropdown = false;
        });
    });
     // End tax list


    // cat list
    $scope.CategoryList = [];
    $scope.CategorySearch = "";
    $scope.showCategoryDropdown = false;
    $scope.loadCategoryList = function () {
        projectService.getallList("1","cat","0").then(function (response) {
            $scope.CategoryList = response.data.d;
        }).catch(function (error) {
            debugger;
            alert("HTTP ERROR: " + (error.statusText || error.message || "Unknown error"));
            console.error("HTTP ERROR:", error);
        });
    };
    $scope.loadCategoryList();
    $scope.selectCategory = function (item) {
        $scope.newitem.CategorySearch = item.name;
        $scope.newitem.categoryid = item.id;      
        $scope.showCategoryDropdown = false;
    };
    $scope.filterCategory = function (item) {
        return $scope.commonDropdownFilter(item, $scope.newitem.CategorySearch, "name");
    };
     // End cat list  


    // UnitList
    $scope.UnitList = [];
    $scope.Unitval = "";
    $scope.showUnitDropdown = false;
    $scope.loadUnitList = function () {
        projectService.getallList("1", "Unit", "0").then(function (response) {
            $scope.UnitList = response.data.d;
        }).catch(function (error) {
            debugger;
            alert("HTTP ERROR: " + (error.statusText || error.message || "Unknown error"));
            console.error("HTTP ERROR:", error);
        });
    };
    $scope.loadUnitList();
    $scope.selectUnit = function (item) {
        $scope.newitem.Unitval = item.name;
        $scope.newitem.unitvalue = item.id;
        $scope.showUnitDropdown = false;
    };
    $scope.filterUnit = function (item) {
        return $scope.commonDropdownFilter(item, $scope.newitem.Unitval, "name");
    };


    $scope.Unitval2 = "";
    $scope.showUnitDropdown2 = false;
    $scope.selectUnit2 = function (item) {
        $scope.newitem.Unitval2 = item.name;
        $scope.newitem.unitvalue2 = item.id;
        $scope.showUnitDropdown2 = false;
    };
    $scope.filterUnit2 = function (item) {
        return $scope.commonDropdownFilter(item, $scope.newitem.Unitval2, "name");
    };
     // End UnitList

      // Common Dropdown filter
    $scope.commonDropdownFilter = function (item, modelValue, displayField) {
        if (!$scope.dropdownTyping) return true;
        if (!modelValue) return true;
        var q = modelValue.toString().toLowerCase().trim();
        var value = ((item && item[displayField]) || "").toString().toLowerCase();
        return value.indexOf(q) !== -1;
    };
     
    ///Upload image Item

    $scope.readURL = function (input) {
        if (input.files && input.files[0]) {
            var reader = new FileReader();

            reader.onload = function (e) {
                $scope.$apply(function () {
                    $scope.newitem.pimg = e.target.result;
                });
            };

            reader.readAsDataURL(input.files[0]);
            $scope.newitem.pimg = input.files[0];
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
                $scope.newitem.pimg = e.target.result;
            });
        };

        reader.readAsDataURL(file);
        $scope.newitem.pimg = file;
        $scope.uploadImage();
    };

    $scope.uploadImage = function () {
        debugger;
        var formData = new FormData();
        formData.append("file", $scope.newitem.pimg);
        projectService.itemimageupload(formData)
       .then(function (response) {
            if (typeof response.data === "string" && response.data.indexOf("Error:") === 0) {
                alert(response.data);
                return;
            }

            $scope.newitem.pimg = response.data;

            if ($scope.newitem.QType == "update") {
               // $scope.saveCustomerauto();
            }
        }, function () {
            alert("Upload Failed");
        });
    };
    //End upload image Item

    $scope.savedata = function () {
        debugger;

        if ($scope.newitem.pname && $scope.newitem.pname.trim() !== '') {

            let payload = angular.copy($scope.newitem);

            //payload.id = toIntOrNull(payload.id);
            payload.ptype = toIntOrNull(payload.ptype);
            payload.exciseproduct = toIntOrNull(payload.exciseproduct);
            payload.issale = toIntOrNull(payload.issale);
            payload.salesaccount = toIntOrNull(payload.salesaccount);
            payload.salesTaxtype = toIntOrNull(payload.salesTaxtype);
            payload.ispurchase = toIntOrNull(payload.ispurchase);
            payload.Purchaseaccount = toIntOrNull(payload.Purchaseaccount);
            payload.PurchaseTaxtype = toIntOrNull(payload.PurchaseTaxtype);
            payload.isTrackInventory = toIntOrNull(payload.isTrackInventory);
            payload.trackAccount = toIntOrNull(payload.trackAccount);
            payload.subcategory2id = toIntOrNull(payload.subcategory2id);
            payload.subcategory3id = toIntOrNull(payload.subcategory3id);
            payload.subcategory4id = toIntOrNull(payload.subcategory4id);
            payload.unitvalue = toIntOrNull(payload.unitvalue);
            payload.unitvalue2 = toIntOrNull(payload.unitvalue2);
            payload.istaxinclusive = toIntOrNull(payload.istaxinclusive);
            payload.Wholesale_istaxinclusive = toIntOrNull(payload.Wholesale_istaxinclusive);
            payload.Franchise_istaxinclusive = toIntOrNull(payload.Franchise_istaxinclusive);
            payload.groupId = toIntOrNull(payload.groupId);
            payload.IsCriticalItem = toIntOrNull(payload.IsCriticalItem);
            payload.HandleType = toIntOrNull(payload.HandleType);
            payload.isPriceMaking = toIntOrNull(payload.isPriceMaking);
            payload.ddlforgin = toIntOrNull(payload.ddlforgin);
            payload.ddlinventorytype = toIntOrNull(payload.ddlinventorytype);
            payload.issalepurchasesame = toIntOrNull(payload.issalepurchasesame);

           // console.log("newitem payload =", payload);

            $scope.isSaving = true;

            projectService.savedataitem(payload)
                .then(function (response) {
                    let result = response.data.d;

                    if (result === "success") {
                        $scope.showToast("✔ product saved successfully!", 4000);
                        $scope.projectState.showlist = true;
                        $scope.newitem = angular.copy($scope.defaultnewitem);
                        $scope.newitem.QType = "insert";
                        $scope.projectState.showAddForm = false;
                        $scope.loadproduct();
                        window.location.hash = "#";
                    } else {
                        alert("Error: " + result);
                    }
                })
                .catch(function (error) {
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
                    $scope.isSaving = false;
                });
        }
        else {
            $scope.showToasterror("Please Enter Product Name!", 4000);
            document.getElementById("txtpname").focus();
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

    function toIntOrNull(val) {
        return (val === undefined || val === null || val === "") ? null : parseInt(val, 10);
    }
});
