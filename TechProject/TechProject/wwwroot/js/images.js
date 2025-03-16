document.addEventListener("DOMContentLoaded", function () {
    let selectedFiles = []; // الصور الجديدة المضافة
    let deletedImageIds = []; // الصور التي تم حذفها
    const savedImages = document.querySelectorAll(".image-container");

    // تحديد الصورة كـ Header Image عند النقر
    savedImages.forEach(imageContainer => {
        imageContainer.addEventListener("click", function () {
            savedImages.forEach(img => img.classList.remove("header-selected"));
            imageContainer.classList.add("header-selected");
            const imageUrl = imageContainer.querySelector("img").getAttribute("src");
            document.getElementById("headerImageUrlInput").value = imageUrl;
        });
    });

    // تمييز الصورة المحفوظة عند تحميل الصفحة
    const headerImageUrl = document.getElementById("headerImageUrlInput").value;
    if (headerImageUrl) {
        savedImages.forEach(imageContainer => {
            const img = imageContainer.querySelector("img");
            if (img.getAttribute("src") === headerImageUrl) {
                imageContainer.classList.add("header-selected");
            }
        });
    }

    // إظهار الأزرار عند النقر على الصورة
    document.addEventListener("click", function (e) {
        if (e.target.classList.contains("preview-image")) {
            document.querySelectorAll(".action-buttons").forEach(btn => btn.style.display = "none");
            e.target.closest(".image-container").querySelector(".action-buttons").style.display = "block";
        } else {
            document.querySelectorAll(".action-buttons").forEach(btn => btn.style.display = "none");
        }
    });

    // اختيار الصورة كـ Header Image
    document.addEventListener("click", function (e) {
        if (e.target.closest(".select-header-image")) {
            let selectedImageUrl = e.target.closest(".select-header-image").dataset.url;
            document.getElementById("headerImageUrlInput").value = selectedImageUrl;
            document.querySelectorAll(".header-selected").forEach(img => img.classList.remove("header-selected"));
            e.target.closest(".image-container").classList.add("header-selected");
        }
    });

    // إضافة ومعاينة الصور الجديدة
    document.getElementById("imageInput").addEventListener("change", function (event) {
        let files = Array.from(event.target.files);
        files.forEach(file => {
            let reader = new FileReader();
            reader.onload = function (e) {
                let index = selectedFiles.length; // تحديد الفهرس الجديد
                selectedFiles.push(file);
                let imgElement = `
                    <div class="image-container" data-index="${index}">
                        <img src="${e.target.result}" class="border img-thumbnail preview-image" />
                        <div class="action-buttons" style="display: none;">
                            <button type="button" class="btn btn-danger btn-sm delete-unsaved-image" data-index="${index}">
                                <i class="fa fa-trash" aria-hidden="true"></i>
                            </button>
                            <button type="button" class="btn btn-primary btn-sm select-header-image" data-url="${e.target.result}">
                               <i class="fa fa-star" aria-hidden="true"></i>
                            </button>
                        </div>
                    </div>`;
                document.getElementById("unsavedImages").insertAdjacentHTML("beforeend", imgElement);
            };
            reader.readAsDataURL(file);
        });

        // اختيار أول صورة كـ Header Image تلقائيًا
        if (files.length > 0 && !document.getElementById("headerImageUrlInput").value) {
            document.getElementById("headerImageUrlInput").value = URL.createObjectURL(files[0]);
        }
    });

    // حذف صورة غير محفوظة
    document.addEventListener("click", function (e) {
        if (e.target.closest(".delete-unsaved-image")) {
            let index = e.target.closest(".delete-unsaved-image").dataset.index;
            selectedFiles.splice(index, 1);
            e.target.closest(".image-container").remove();
            updateFileInput();
        }
    });

    // حذف صورة محفوظة من قاعدة البيانات
    document.addEventListener("click", function (e) {
        if (e.target.closest(".delete-saved-image")) {
            let imageId = e.target.closest(".delete-saved-image").dataset.id;
            let imageElement = e.target.closest(".image-container");

            fetch(`/Admin/Product/DeleteImage/${imageId}`, { method: "POST" })
                .then(response => response.json())
                .then(data => {
                    if (data.success) {
                        imageElement.remove();
                        deletedImageIds.push(imageId);
                    } else {
                        alert(data.message);
                    }
                })
                .catch(() => alert("Error deleting image."));
        }
    });

    // تحديث المدخلات بعد الحذف
    function updateFileInput() {
        let fileList = new DataTransfer();
        selectedFiles.forEach(file => fileList.items.add(file));
        document.getElementById("imageInput").files = fileList.files;
    }
});
