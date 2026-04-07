let tarefaParaFinalizar = null;
const modal = document.getElementById('confirmModal');

// Abre o modal e guarda o id da tarefa
window.confirmarFinalizacao = function (id) {
    tarefaParaFinalizar = id;
    modal.classList.add('active');
};

// Confirma a finalização
document.getElementById('confirmFinalizar').addEventListener('click', async function () {
    if (tarefaParaFinalizar) {
        try {
            const response = await fetch(`/Home/EndTask/${tarefaParaFinalizar}`, {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' }
            });
            const data = await response.json();
            if (data.success) {
                showMessage(null, data.message, 3000, '/');
            } else {
                if (data.redirect) {
                    showMessage('warning', data.message, 5000, '/Account/Login');
                } else {
                    showMessage('warning', data.message, 5000, null);
                }
            }
        } catch (error) {
            showMessage('error', 'Erro de conexão. Tente novamente.', 5000, null);
        }
    }
    modal.classList.remove('active');
    tarefaParaFinalizar = null;
});

// Cancela a finalização
document.getElementById('cancelFinalizar').addEventListener('click', function () {
    modal.classList.remove('active');
    tarefaParaFinalizar = null;
});

// Fecha o modal clicando fora
window.addEventListener('click', function (e) {
    if (e.target === modal) {
        modal.classList.remove('active');
        tarefaParaFinalizar = null;
    }
});