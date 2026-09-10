
CREATE DATABASE Project;
GO

USE Project;
GO

CREATE TABLE user_table
(
    userid      INT             PRIMARY KEY,
    firstname   VARCHAR(50)     NOT NULL,
    lastname    VARCHAR(50)     NOT NULL,
    email       VARCHAR(100)    NOT NULL,
    gender      VARCHAR(10)     NULL,
    password    VARCHAR(50)     NOT NULL,
    role        VARCHAR(20)     NOT NULL,
    dob         DATE            NULL
);
GO

CREATE TABLE course_table
(
    courseid        INT             PRIMARY KEY,
    coursename      VARCHAR(50)     NOT NULL,
    coursetype      VARCHAR(50)     NOT NULL,
    coursetime      VARCHAR(100)    NOT NULL,
    courseprice     DECIMAL(10,2)   NOT NULL DEFAULT 0,
    discountpercent DECIMAL(5,2)    NOT NULL DEFAULT 0
);
GO

CREATE TABLE cart_table
(
    cartid      INT IDENTITY(1,1) PRIMARY KEY,
    userid      INT             NOT NULL,
    courseid    INT             NOT NULL,
    status      VARCHAR(20)     NOT NULL DEFAULT 'Pending',
    date_added  DATETIME        NOT NULL DEFAULT GETDATE(),

    CONSTRAINT FK_cart_user
        FOREIGN KEY (userid) REFERENCES user_table(userid),

    CONSTRAINT FK_cart_course
        FOREIGN KEY (courseid) REFERENCES course_table(courseid)
);
GO

INSERT INTO user_table
(userid, firstname, lastname, email, gender, password, role)
VALUES
(1, 'System', 'Admin', 'superadmin@gmail.com', 'Male', 'admin123', 'Super Admin');

INSERT INTO user_table
(userid, firstname, lastname, email, gender, password, role)
VALUES
(2, 'Course', 'Admin', 'admin@gmail.com', 'Male', 'admin456', 'Admin');
GO


INSERT INTO course_table (courseid, coursename, coursetype, coursetime, courseprice, discountpercent) VALUES
(11, 'Java', 'Offline Class', 'Sun 8:0 - Sun 10:20 AM', 3500.00, 10.00),
(12, 'Python', 'Online Class', 'Mon 11:20 - Mon 12:50 PM', 3800.00, 10.00),
(13, 'C++', 'Offline Class', 'Sun 11:20 - Sun 12:50 PM', 2500.00, 10.00),
(14, 'UI/UX Design', 'Offline Class', 'Sun 11:20 - Sun 12:50 PM', 4500.00, 10.00),
(15, 'Software Design', 'Online Class', 'Mon 11:20 - Mon 12:50 PM', 4500.00, 10.00),
(16, 'C#', 'Online Class', 'Mon 8:0 - Mon 9:30 AM', 2500.00, 10.00),
(17, 'Node.Js', 'Offline Class', 'Mon 8:0 - Mon 9:30 AM', 5000.00, 10.00),
(18, 'Graphics Design', 'Online Class', 'Tue 8:0 - Tue 10:0 AM', 4800.00, 10.00);
GO

SELECT * FROM user_table;
SELECT * FROM course_table;
SELECT * FROM cart_table;
