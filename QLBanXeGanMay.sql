CREATE DATABASE QLBanXeGanMay1; 
GO 
USE QLBanXeGanMay1; 
GO 
-- Table: KHACHHANG 
CREATE TABLE KHACHHANG 
( 
MaKH INT IDENTITY(1,1), 
HoTen NVARCHAR(50) NOT NULL, 
Taikhoan VARCHAR(50) UNIQUE, 
Matkhau VARCHAR(50) NOT NULL, 
Email VARCHAR(100) UNIQUE, 
DiachiKH NVARCHAR(200), 
DienthoaiKH VARCHAR(50), 
Ngaysinh DATETIME, 
CONSTRAINT PK_Khachhang PRIMARY KEY (MaKH) 
); 
GO 
-- Table: LOAIXE 
CREATE TABLE LOAIXE 
( 
MaLX INT IDENTITY(1,1), 
TenLoaiXe NVARCHAR(50) NOT NULL, 
CONSTRAINT PK_LoaiXe PRIMARY KEY (MaLX) 
); 
GO 
-- Table: NHAPHANPHOI 
CREATE TABLE NHAPHANPHOI 
( 
MaNPP INT IDENTITY(1,1), 
TenNPP NVARCHAR(50) NOT NULL, 
Diachi NVARCHAR(200), 
DienThoai VARCHAR(50), 
CONSTRAINT PK_NhaPhanPhoi PRIMARY KEY (MaNPP) 
); 
GO 
-- Table: XEGANMAY 
CREATE TABLE XEGANMAY 
( 
MaXe INT IDENTITY(1,1), 
TenXe NVARCHAR(100) NOT NULL, 
Giaban DECIMAL(18,0) CHECK (Giaban >= 0), 
Mota NVARCHAR(MAX), 
Anhbia VARCHAR(50), 
Ngaycapnhat DATETIME, 
Soluongton INT, 
MaLX INT, 
MaNPP INT, 
CONSTRAINT PK_XeGanMay PRIMARY KEY (MaXe), 
CONSTRAINT FK_LoaiXe FOREIGN KEY (MaLX) REFERENCES LOAIXE(MaLX), 
CONSTRAINT FK_HangSanXuat FOREIGN KEY (MaNPP) REFERENCES NHAPHANPHOI(MaNPP) 
); 
GO 
-- Table: HANGSANXUAT 
CREATE TABLE HANGSANXUAT 
( 
MaHSX INT IDENTITY(1,1), 
TenHSX NVARCHAR(50) NOT NULL, 
Diachi NVARCHAR(100), 
    Tieusu NVARCHAR(MAX), 
    Dienthoai VARCHAR(50), 
    CONSTRAINT PK_HangSX PRIMARY KEY (MaHSX) 
); 
GO 
 -- Table: SANXUATXE 
CREATE TABLE SANXUATXE 
( 
    MaXe INT NOT NULL,      -- Mã xe, liên kết tới bảng XEGANMAY 
    MaHSX INT NOT NULL,     -- Mã nhà sản xuất, liên kết tới bảng HANGSANXUAT 
    NgaySX DATE,            -- Ngày sản xuất 
    SoLuong INT CHECK (SoLuong >= 0), -- Số lượng xe sản xuất (>= 0) 
    CONSTRAINT PK_SanXuatXe PRIMARY KEY (MaXe, MaHSX), -- Khóa chính là tổ hợp MaXe  và MaHSX 
    CONSTRAINT FK_SanXuatXe_Xe FOREIGN KEY (MaXe) REFERENCES XEGANMAY(MaXe), 
    CONSTRAINT FK_SanXuatXe_HSX FOREIGN KEY (MaHSX) REFERENCES HANGSANXUAT(MaHSX) 
); 
GO 
 -- Table: DONDATHANG 
CREATE TABLE DONDATHANG		
( 
    MaDonHang INT IDENTITY(1,1), 
    Dathanhtoan BIT, 
    Tinhtranggiaohang BIT, 
    Ngaydat DATETIME, 
    Ngaygiao DATETIME, 
    MaKH INT, 
    CONSTRAINT PK_DonDatHang PRIMARY KEY (MaDonHang), 
    CONSTRAINT FK_Khachhang FOREIGN KEY (MaKH) REFERENCES KHACHHANG(MaKH) 
); 
GO 
 -- Table: CHITIETDONTHANG 
CREATE TABLE CHITIETDONTHANG 
( 
    MaDonHang INT, 
    MaXe INT, 
    Soluong INT CHECK (Soluong > 0), 
    Dongia DECIMAL(18,0) CHECK (Dongia >= 0), 
    CONSTRAINT PK_CTDatHang PRIMARY KEY (MaDonHang, MaXe), 
    CONSTRAINT FK_Donhang FOREIGN KEY (MaDonHang) REFERENCES DONDATHANG(MaDonHang), 
    CONSTRAINT FK_Xe FOREIGN KEY (MaXe) REFERENCES XEGANMAY(MaXe) 
); 
GO 
 
-- Chèn dữ liệu vào bảng KHACHHANG
INSERT INTO KHACHHANG (HoTen, Taikhoan, Matkhau, Email, DiachiKH, DienthoaiKH, Ngaysinh)
VALUES
('Nguyen Van A', 'nguyenvana', 'matkhau123', 'nguyenvana@example.com', 'Hanoi, Vietnam', '0123456789', '1990-01-01'),
('Nguyen Van C', 'nguyenvanc', 'matkhau123', 'nguyenvanc@example.com', 'Phu Yen, Vietnam', '0123456456', '1993-02-09'),
('Le Thi B', 'lethib', 'matkhau456', 'lethib@example.com', 'Ho Chi Minh City, Vietnam', '0987654321', '1985-05-15');

-- Chèn dữ liệu vào bảng LOAIXE
INSERT INTO LOAIXE (TenLoaiXe)
VALUES
('Xe máy tay ga'),
('Xe máy côn tay'),
('Xe máy phân khối lớn'),
('Xe máy điện');

-- Chèn dữ liệu vào bảng NHAPHANPHOI
INSERT INTO NHAPHANPHOI (TenNPP, Diachi, DienThoai)
VALUES
('Công ty TNHH Xe Máy Việt', 'Ho Chi Minh City, Vietnam', '0909887766'),
('Công ty CP Xe Gắn Máy Đạt Tâm', 'Hanoi, Vietnam', '0912333444'),
('Công ty TNHH Xe Máy Cao Cấp', 'Da Nang, Vietnam', '0988776655');

-- Chèn dữ liệu vào bảng XEGANMAY
INSERT INTO XEGANMAY (TenXe, Giaban, Mota, Anhbia, Ngaycapnhat, Soluongton, MaLX, MaNPP)
VALUES
('Honda Vision', 40000000, 'Xe tay ga cao cấp, thiết kế sang trọng', 'honda_vision.jpg', '2024-11-17', 100, 1, 1),
('Honda Gold Wing 2024', 1230000000, 'Chiếc xe Gold Wing Tour đã tiên phong trong việc trang bị túi khí trên xe mô tô từ hơn 15 năm trước. Túi khí đảm bảo an toàn cho người lái được trang bị độc nhất trên mẫu Gold Wing Tour DCT.', 'honda_goldwing2024.jpg', '2024-11-20', 50, 3, 1),
('Honda Transalp', 350000000, 'Một trong những điểm đáng chú ý nhất về Transalp chính là động cơ. Công suất tối đa 67,5kW, mô-men xoắn cực đại 75Nm - đều là những con số cực kỳ ấn tượng cho cỗ máy dung tích 755cc. Nhưng không dừng lại ở đó, khối động cơ xi-lanh đôi song song 8 van này 
sở hữu công nghệ nạp khí xoáy (Air Vortex) giúp xe phản ứng mượt mà ở vòng tua thấp và trung bình, và ở vòng tua máy cao, lớp mạ Ni-SiC chuyên dụng trên xy lanh hỗ trợ giảm ma sát hiệu quả và tăng cường sức mạnh động cơ. Nhờ đó, Transalp trở thành mẫu xe 
lý tưởng trên cả đường đất và đường nhựa, bất kể trọng lượng của người lái hay hành lý mang theo.', 'honda_transalp.jpg', '2024-12-22', 20, 3, 3),
('Honda ICON e:', 30000000, 'CON e: với thiết kế trẻ trung và mang tính biểu tượng, có thể khiến khách hàng dễ dàng nhận ra và nổi bật giữa đám đông. ', 'honda_ICONe.jpg', '2024-11-15', 200, 4, 1),
('Honda Vario 125', 41000000, 'Xe tay ga cao cấp, thiết kế sang trọng', 'honda_vario125.jpg', '2024-11-20', 100, 1, 2),
('Honda Air Blade 160', 45000000, 'Xe tay ga cao cấp, thiết kế sang trọng', 'honda_airblade160.jpg', '2024-01-10', 100, 1, 2),

('Honda SH 125i', 80000000, 'Xe tay ga cao cấp, thiết kế sang trọng', 'honda_sh125i.jpg', '2024-11-15', 100, 1, 1),
('Yamaha Exciter 150', 47000000, 'Xe côn tay thể thao, động cơ mạnh mẽ', 'yamaha_exciter150.jpg', '2024-11-10', 120, 2, 2),
('Honda Wave Alpha', 20000000, 'Xe máy phổ thông, tiết kiệm nhiên liệu', 'honda_wave_alpha.jpg', '2024-11-08', 200, 1, 3),
('VinFast Klara', 45000000, 'Xe máy điện, công nghệ tiên tiến, tiết kiệm năng lượng', 'vinfast_klara.jpg', '2024-11-05', 80, 4, 3),
('Suzuki Raider R150', 55000000, 'Xe côn tay thể thao, thiết kế thể thao mạnh mẽ', 'suzuki_raider.jpg', '2024-11-07', 50, 2, 2),
('Honda Lead 125', 58000000, 'Xe tay ga tiện dụng cho phụ nữ, động cơ mạnh mẽ', 'honda_lead_125.jpg', '2024-11-22', 150, 1, 1),
('Yamaha NVX 155', 72000000, 'Xe tay ga thể thao, thiết kế mạnh mẽ, tiện ích cao cấp', 'yamaha_nvx_155.jpg', '2024-11-20', 200, 1, 2),
('Piaggio Liberty 125', 65000000, 'Xe tay ga sang trọng, thiết kế thanh lịch, phù hợp với thành phố', 'piaggio_liberty_125.jpg', '2024-11-18', 180, 1, 3),
('Kawasaki Ninja 400', 175000000, 'Xe phân khối lớn, thể thao, mạnh mẽ, thích hợp cho người yêu thích tốc độ', 'kawasaki_ninja_400.jpg', '2024-11-15', 60, 3, 2),
('Suzuki GSX-R150', 75000000, 'Xe thể thao, thiết kế đậm chất thể thao, động cơ mạnh mẽ', 'suzuki_gsxr150.jpg', '2024-11-14', 100, 2, 2),
('Honda CBR 150R', 90000000, 'Xe phân khối lớn, thiết kế thể thao, khả năng vận hành mạnh mẽ', 'honda_cbr_150r.jpg', '2024-11-10', 50, 3, 1),
('Vespa Sprint 125', 85000000, 'Xe tay ga cao cấp, thiết kế cổ điển, phù hợp cho cả nam và nữ', 'vespa_sprint_125.jpg', '2024-11-08', 120, 4, 3),
('Yamaha FZ-S FI', 64000000, 'Xe côn tay thể thao, động cơ mạnh mẽ, thiết kế năng động', 'yamaha_fz_s_fi.jpg', '2024-11-05', 80, 2, 1),
('Honda CRF 250L', 160000000, 'Xe phân khối lớn, chuyên dụng cho địa hình xấu, dễ dàng vượt qua mọi thử thách', 'honda_crf250l.jpg', '2024-11-02', 40, 3, 2),
('SYM Shark 125', 48000000, 'Xe tay ga mạnh mẽ, thiết kế thể thao, giá cả phải chăng', 'sym_shark_125.jpg', '2024-11-01', 200, 1, 1);

-- Chèn dữ liệu vào bảng HANGSANXUAT
INSERT INTO HANGSANXUAT (TenHSX, Diachi, Tieusu, Dienthoai)
VALUES
('Honda', 'Japan', 'Honda là một trong những nhà sản xuất xe máy hàng đầu thế giới, với nhiều dòng xe nổi bật như xe tay ga, xe phân khối lớn, và xe thể thao.', '0341234567'),
('Yamaha', 'Japan', 'Yamaha là một trong những nhà sản xuất xe máy nổi tiếng, chuyên sản xuất các dòng xe tay ga và xe thể thao. Họ luôn dẫn đầu về công nghệ và hiệu suất.', '0987654321'),
('Piaggio', 'Italy', 'Piaggio là hãng sản xuất xe nổi tiếng với các dòng xe tay ga cao cấp, đặc biệt là các dòng xe Vespa mang tính biểu tượng. Họ được biết đến với thiết kế thanh lịch và chất lượng vượt trội.', '0345678910'),
('Kawasaki', 'Japan', 'Kawasaki là thương hiệu nổi tiếng chuyên sản xuất xe phân khối lớn, đặc biệt là các dòng xe thể thao với động cơ mạnh mẽ và thiết kế hiện đại.', '0912345678'),
('Suzuki', 'Japan', 'Suzuki là nhà sản xuất xe máy và xe ô tô hàng đầu Nhật Bản. Họ sản xuất nhiều dòng xe từ xe thể thao đến xe phổ thông với chất lượng đáng tin cậy.', '0321456789'),
('VinFast', 'Vietnam', 'VinFast là hãng xe Việt Nam, chuyên sản xuất xe máy điện và ô tô. Họ nổi bật với công nghệ hiện đại và các sản phẩm thân thiện với môi trường.', '0901010101'),
('Harley-Davidson', 'USA', 'Harley-Davidson là thương hiệu nổi tiếng với các dòng xe mô tô phân khối lớn, mang đến trải nghiệm lái xe mạnh mẽ và độc đáo.', '0343232323'),
('BMW Motorrad', 'Germany', 'BMW Motorrad chuyên sản xuất các dòng xe phân khối lớn, xe touring và xe thể thao, mang lại hiệu suất và sự sang trọng cao cấp.', '0918989898');

-- Chèn dữ liệu vào bảng SANXUATXE
INSERT INTO SANXUATXE (MaXe, MaHSX, NgaySX, SoLuong)
VALUES
(1, 1, '2024-10-15', 300),
(2, 2, '2024-09-10', 200),
(3, 1, '2024-08-25', 400),
(4, 3, '2024-07-30', 150),
(5, 2, '2024-09-05', 100);

-- Chèn dữ liệu vào bảng DONDATHANG
INSERT INTO DONDATHANG (Dathanhtoan, Tinhtranggiaohang, Ngaydat, Ngaygiao, MaKH)
VALUES
(0, 1, '2024-11-01', '2024-11-05', 1),
(1, 0, '2024-11-03', '2024-11-07', 2),
(1, 1, '2024-11-04', '2024-11-10', 3);

-- Chèn dữ liệu vào bảng CHITIETDONTHANG
INSERT INTO CHITIETDONTHANG (MaDonHang, MaXe, Soluong, Dongia)
VALUES
(4, 1, 1, 80000000),  -- Đơn hàng 1, xe 1
(5, 3, 1, 20000000),  -- Đơn hàng 1, xe 3
(5, 2, 2, 47000000),  -- Đơn hàng 2, xe 2
(6, 4, 3, 45000000);  -- Đơn hàng 3, xe 4
