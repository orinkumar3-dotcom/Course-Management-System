
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

SELECT userid, password, role
FROM user_table
WHERE userid = @userid AND password = @password;


INSERT INTO user_table (userid, firstname, lastname, email, password, role, dob)
VALUES (@userid, @firstname, @lastname, @email, @password, @role, @dob);

SELECT * FROM course_table;

SELECT * FROM course_table WHERE courseid = @courseid;

SELECT COUNT(*) FROM course_table WHERE courseid = @courseid;

INSERT INTO course_table (courseid, coursename, coursetype, coursetime, courseprice)
VALUES (@courseid, @coursename, @coursetype, @coursetime, @courseprice);

UPDATE course_table
SET coursename = @coursename,
    coursetype = @coursetype,
    coursetime = @coursetime,
    courseprice = @courseprice
WHERE courseid = @courseid;

SELECT courseid, coursename, coursetype, coursetime, courseprice, discountpercent,
       CAST(courseprice - (courseprice * discountpercent / 100) AS DECIMAL(10,2)) AS Finalprice
FROM course_table;

SELECT * FROM course_table WHERE courseid = @courseid;

SELECT COUNT(*) FROM course_table WHERE courseid = @courseid;

INSERT INTO course_table (courseid, coursename, coursetype, coursetime, courseprice)
VALUES (@courseid, @coursename, @coursetype, @coursetime, @courseprice);

UPDATE course_table
SET coursename = @coursename,
    coursetype = @coursetype,
    coursetime = @coursetime,
    courseprice = @courseprice
WHERE courseid = @courseid;

UPDATE course_table
SET discountpercent = @discount
WHERE courseid = @courseid;

UPDATE course_table
SET discountpercent = @discount;


DELETE FROM course_table WHERE courseid = @courseid;


SELECT userid, firstname, lastname, email, dob, password
FROM user_table
WHERE role = 'Student';


SELECT userid, firstname, lastname, email, dob, password
FROM user_table
WHERE role = 'Student' AND userid = @userid;


UPDATE user_table
SET firstname = @firstname,
    lastname = @lastname,
    email = @email,
    dob = @dob
WHERE userid = @userid;


UPDATE user_table
SET password = @newpassword
WHERE userid = @userid;

DELETE FROM user_table WHERE userid = @userid;


SELECT c.cartid, c.userid, u.firstname, u.lastname,
       c.courseid, co.coursename, co.courseprice,
       c.status, c.date_added
FROM cart_table c
JOIN user_table u    ON c.userid   = u.userid
JOIN course_table co ON c.courseid = co.courseid
WHERE c.status = 'Pending'
ORDER BY c.date_added;


UPDATE cart_table SET status = 'Approved' WHERE cartid = @cartid;

UPDATE cart_table SET status = 'Approved' WHERE status = 'Pending';

UPDATE cart_table SET status = 'Rejected' WHERE cartid = @cartid;


SELECT ISNULL(SUM(co.courseprice - (co.courseprice * co.discountpercent / 100)), 0)
FROM cart_table c
JOIN course_table co ON c.courseid = co.courseid
WHERE c.status = 'Purchased';


SELECT COUNT(*) FROM cart_table WHERE status = 'Purchased';


SELECT COUNT(DISTINCT userid) FROM cart_table WHERE status = 'Purchased';


SELECT COUNT(*) FROM cart_table WHERE status = 'Pending';


SELECT u.firstname, u.lastname, co.coursename,
       CAST(co.courseprice - (co.courseprice * co.discountpercent / 100) AS DECIMAL(10,2)) AS Finalprice,
       c.date_added
FROM cart_table c
JOIN user_table u    ON c.userid   = u.userid
JOIN course_table co ON c.courseid = co.courseid
WHERE c.status = 'Purchased'
ORDER BY c.date_added DESC;


SELECT co.courseid,
       co.coursename,
       COUNT(*) AS TimesPurchased,
       SUM(co.courseprice - (co.courseprice * co.discountpercent / 100)) AS TotalRevenue
FROM cart_table c
JOIN course_table co ON c.courseid = co.courseid
WHERE c.status = 'Purchased'
GROUP BY co.courseid, co.coursename
ORDER BY TotalRevenue DESC;


SELECT courseid, coursename, coursetype, coursetime, courseprice, discountpercent,
       CAST(courseprice - (courseprice * discountpercent / 100) AS DECIMAL(10,2)) AS Finalprice
FROM course_table;

SELECT * FROM course_table WHERE courseid = @courseid;


SELECT COUNT(*) FROM cart_table
WHERE userid = @userid AND courseid = @courseid
AND status IN ('Pending', 'Approved', 'Purchased');


INSERT INTO cart_table (userid, courseid, status, date_added)
VALUES (@userid, @courseid, 'Pending', GETDATE());


SELECT ROW_NUMBER() OVER (PARTITION BY c.userid ORDER BY c.cartid) AS DisplayNo,
       c.cartid, c.courseid, co.coursename, co.courseprice, co.discountpercent,
       CAST(co.courseprice - (co.courseprice * co.discountpercent / 100) AS DECIMAL(10,2)) AS Finalprice,
       c.status, c.date_added
FROM cart_table c
JOIN course_table co ON c.courseid = co.courseid
WHERE c.userid = @userid;


DELETE FROM cart_table
WHERE cartid = @cartid AND userid = @userid;


UPDATE cart_table SET status = 'Purchased'
WHERE cartid = @cartid AND userid = @userid AND status = 'Approved';


UPDATE cart_table SET status = 'Purchased'
WHERE userid = @userid AND status = 'Approved';


SELECT userid, firstname, lastname, email, password, dob
FROM user_table
WHERE userid = @userid;


UPDATE user_table
SET firstname = @firstname,
    lastname = @lastname,
    email = @email,
    dob = @dob
WHERE userid = @userid;

UPDATE user_table
SET password = @newpassword
WHERE userid = @userid;


SELECT u.userid, u.firstname, u.lastname, u.email,
       co.courseid, co.coursename, co.courseprice, co.discountpercent,
       CAST(co.courseprice - (co.courseprice * co.discountpercent / 100) AS DECIMAL(10,2)) AS discountprice,
       c.status, c.date_added
FROM cart_table c
JOIN user_table u    ON c.userid   = u.userid
JOIN course_table co ON c.courseid = co.courseid
WHERE c.userid = @userid AND c.status = 'Purchased'
ORDER BY c.date_added DESC;


SELECT u.userid, u.firstname, u.lastname, u.email,
       co.courseid, co.coursename, co.courseprice, co.discountpercent,
       CAST(co.courseprice - (co.courseprice * co.discountpercent / 100) AS DECIMAL(10,2)) AS discountprice,
       c.status, c.date_added
FROM cart_table c
JOIN user_table u    ON c.userid   = u.userid
JOIN course_table co ON c.courseid = co.courseid
WHERE c.cartid IN (@id0, @id1 


SELECT c.cartid, c.userid, u.firstname, u.lastname, u.email,
       c.courseid, co.coursename, co.courseprice, co.discountpercent,
       CAST(co.courseprice - (co.courseprice * co.discountpercent / 100) AS DECIMAL(10,2)) AS Finalprice,
       c.status, c.date_added
FROM cart_table c
JOIN user_table u    ON c.userid   = u.userid
JOIN course_table co ON c.courseid = co.courseid
WHERE c.cartid IN (@id0, @id1, @id2 
