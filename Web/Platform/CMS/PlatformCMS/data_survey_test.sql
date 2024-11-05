INSERT INTO Questions (survey_id, text, question_type, [order])
VALUES 
    (1, 'Bạn có hài lòng với sản phẩm của chúng tôi không?', 'multiple_choice', 1),
    (1, 'Bạn sẽ giới thiệu sản phẩm của chúng tôi cho người khác không?', 'yes_no', 2),
    (2, 'Bạn nghĩ sao về tính năng mới của sản phẩm?', 'open_ended', 1),
    (2, 'Bạn có muốn thấy tính năng nào khác trong sản phẩm không?', 'open_ended', 2);

INSERT INTO Options (question_id, text, value)
VALUES 
    (6, 'Rất hài lòng', 5),
    (6, 'Hài lòng', 4),
    (6, 'Bình thường', 3),
    (6, 'Không hài lòng', 2),
    (6, 'Rất không hài lòng', 1),
    (7, 'Có', 1),
    (7, 'Không', 0);

INSERT INTO Responses (survey_id, email)
VALUES 
    (1, 'customer1@example.com'),
    (1, 'customer2@example.com'),
    (2, 'customer3@example.com');

INSERT INTO Answers (response_id, question_id, option_id, text_answer)
VALUES 
    (1, 1, 5, NULL),  -- customer1 chọn "Rất không hài lòng" cho câu hỏi 1
    (1, 2, 1, NULL),  -- customer1 chọn "Có" cho câu hỏi 2
    (2, 1, 4, NULL),  -- customer2 chọn "Hài lòng" cho câu hỏi 1
    (2, 2, 1, NULL),  -- customer2 chọn "Có" cho câu hỏi 2
    (3, 1, 4, NULL),  -- customer3 chọn "Tốt" cho câu hỏi 3
    (3, 2, 3, NULL);  -- customer3 chọn "Thêm tính năng A" cho câu hỏi 4
