CREATE TABLE cart_table
(
    cartid      INT IDENTITY(1,1) PRIMARY KEY,
    userid      int   NOT NULL,
    courseid    int   NOT NULL,
    status      VARCHAR(20)   NOT NULL DEFAULT 'Pending',
    date_added  DATETIME      NOT NULL DEFAULT GETDATE(),

    CONSTRAINT FK_cart_user
        FOREIGN KEY (userid) REFERENCES user_table(userid),

    CONSTRAINT FK_cart_course
        FOREIGN KEY (courseid) REFERENCES course_table(courseid)
);