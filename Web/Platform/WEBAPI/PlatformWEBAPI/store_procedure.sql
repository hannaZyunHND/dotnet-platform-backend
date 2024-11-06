CREATE PROCEDURE usp_web_InsertEmailSubcriber
    @Email VARCHAR(255)  -- Tham số đầu vào cho địa chỉ email
AS
BEGIN
    -- Kiểm tra xem địa chỉ email đã tồn tại trong bảng chưa
    IF EXISTS (SELECT 1 FROM EmailSubscriptions WHERE Email = @Email)
    BEGIN
        -- Nếu đã tồn tại, có thể trả về thông báo hoặc mã lỗi
        RAISERROR('Email đã tồn tại trong hệ thống.', 16, 1);
        RETURN; -- Kết thúc procedure
    END

    -- Chèn địa chỉ email vào bảng EmailSubscriptions
    INSERT INTO EmailSubscriptions (Email)
    VALUES (@Email);
END

CREATE PROCEDURE usp_web_GetAllEmailSubcriber
AS
BEGIN
    SET NOCOUNT ON;  -- Tắt thông báo số hàng ảnh hưởng

    SELECT * FROM EmailSubscriptions;
END
GO

EXEC usp_web_GetAllEmailSubcriber;
