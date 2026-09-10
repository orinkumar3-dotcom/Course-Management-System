USE project;

INSERT INTO user_table
(userid, firstname, lastname, email, gender, password, role)
VALUES
(1, 'System', 'Admin', 'superadmin@gmail.com', 'Male', 'admin123', 'Super Admin');

INSERT INTO user_table
(userid, firstname, lastname, email, gender, password, role)
VALUES
(2, 'Course', 'Admin', 'admin@gmail.com', 'Male', 'admin456', 'Admin');

select *
from user_table