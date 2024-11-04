CREATE TABLE EmailSubscriptions (
    Id INT PRIMARY KEY IDENTITY(1,1),      -- Khóa chính, tự động tăng từ 1
    Email VARCHAR(255) NOT NULL UNIQUE,    -- Địa chỉ email, không null và phải là duy nhất
    CreatedAt DATETIME DEFAULT GETDATE(),   -- Thời gian đăng ký, mặc định là thời gian hiện tại
    UpdatedAt DATETIME DEFAULT GETDATE()    -- Thời gian cập nhật, mặc định là thời gian hiện tại
);
