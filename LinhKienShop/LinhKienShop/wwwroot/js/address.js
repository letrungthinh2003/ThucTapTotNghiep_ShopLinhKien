function loadProvinces(callback) {
    $.get("/Address/GetProvinces")
        .done(function (data) {
            const provinces = data; // Không cần JSON.parse vì jQuery tự parse
            const $province = $("#province");
            $province.empty().append('<option value="">Chọn tỉnh/thành phố</option>');
            provinces.forEach(province => {
                $province.append(`<option value="${province.name}" data-code="${province.code}">${province.name}</option>`);
            });
            if (callback) callback();
        })
        .fail(function () {
            console.error("Lỗi khi tải danh sách tỉnh/thành phố");
            alert("Không thể tải danh sách tỉnh/thành phố. Vui lòng thử lại.");
        });
}

function loadDistricts(provinceName, callback) {
    const provinceCode = $("#province option[value='" + provinceName + "']").data("code");
    if (!provinceCode) {
        $("#district").empty().append('<option value="">Chọn quận/huyện</option>').prop("disabled", true);
        $("#ward").empty().append('<option value="">Chọn phường/xã</option>').prop("disabled", true);
        return;
    }

    $.get("/Address/GetDistricts?provinceCode=" + provinceCode)
        .done(function (data) {
            const province = data; // Không cần JSON.parse
            const districts = province.districts;
            const $district = $("#district");
            $district.empty().append('<option value="">Chọn quận/huyện</option>').prop("disabled", false);
            districts.forEach(district => {
                $district.append(`<option value="${district.name}" data-code="${district.code}">${district.name}</option>`);
            });
            if (callback) callback();
        })
        .fail(function () {
            console.error("Lỗi khi tải danh sách quận/huyện");
            alert("Không thể tải danh sách quận/huyện. Vui lòng thử lại.");
        });
}

function loadWards(districtName, callback) {
    const districtCode = $("#district option[value='" + districtName + "']").data("code");
    if (!districtCode) {
        $("#ward").empty().append('<option value="">Chọn phường/xã</option>').prop("disabled", true);
        return;
    }

    $.get("/Address/GetWards?districtCode=" + districtCode)
        .done(function (data) {
            const district = data; // Không cần JSON.parse
            const wards = district.wards;
            const $ward = $("#ward");
            $ward.empty().append('<option value="">Chọn phường/xã</option>').prop("disabled", false);
            wards.forEach(ward => {
                $ward.append(`<option value="${ward.name}">${ward.name}</option>`);
            });
            if (callback) callback();
        })
        .fail(function () {
            console.error("Lỗi khi tải danh sách phường/xã");
            alert("Không thể tải danh sách phường/xã. Vui lòng thử lại.");
        });
}

$(document).ready(function () {
    $("#province").change(function () {
        const provinceName = $(this).val();
        loadDistricts(provinceName);
        $("#ward").empty().append('<option value="">Chọn phường/xã</option>').prop("disabled", true);
    });

    $("#district").change(function () {
        const districtName = $(this).val();
        loadWards(districtName);
    });
});