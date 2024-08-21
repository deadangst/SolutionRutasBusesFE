function aplicarEstiloMantenimiento() {
    var style = document.createElement('style');
    style.type = 'text/css';
    style.innerHTML = `
        .maintenance-ticket {
            background-color: #ffffff;
            border: 2px dashed #007bff;
            padding: 30px;
            border-radius: 12px;
            max-width: 700px;
            margin: 5% auto;
            box-shadow: 0 0 20px rgba(0, 0, 0, 0.1);
        }

        .maintenance-ticket h1 {
            font-size: 1.75rem;
            color: #007bff;
            margin-bottom: 25px;
            text-align: center;
        }

        .maintenance-ticket label {
            font-weight: bold;
            color: #343a40;
        }

        .maintenance-ticket .form-control[readonly] {
            background-color: #e9ecef;
            border: none;
            font-weight: bold;
            color: #495057;
        }

        .maintenance-ticket .form-control {
            background-color: #f8f9fa;
            border: 1px solid #ced4da;
        }

        .maintenance-ticket .btn-primary {
            background-color: #007bff;
            border-color: #007bff;
            width: 100%;
            font-size: 1.2rem;
            padding: 10px;
        }

        .maintenance-ticket .ticket-footer {
            text-align: center;
            margin-top: 30px;
        }

        .maintenance-ticket .ticket-footer a {
            color: #007bff;
            font-weight: bold;
            text-decoration: none;
        }

        .maintenance-ticket .ticket-footer a:hover {
            text-decoration: underline;
        }

        .dashed-divider {
            border-bottom: 1px dashed #007bff;
            margin: 20px 0;
        }

        .icon-container {
            text-align: center;
            margin-top: 20px;
        }

        .icon-container i {
            font-size: 5rem;
            color: #007bff;
        }
    `;
    document.getElementsByTagName('head')[0].appendChild(style);
}
