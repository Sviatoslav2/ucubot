USE ucubot;
ALTER TABLE lesson_signal DROP COLUMN user_id;
ALTER TABLE lesson_signal ADD student_id INT NOT NULL;
ALTER TABLE lesson_signal ADD CONSTRAINT fk FOREIGN KEY (student_id) REFERENCES student(id) ON UPDATE RESTRICT ON DELETE RESTRICT;
