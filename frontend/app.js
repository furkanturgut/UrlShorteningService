document.addEventListener('DOMContentLoaded', function() {
    const API_BASE_URL = 'http://localhost:5024'; // Backend API URL
    const REDIRECT_BASE_URL = 'http://localhost:5024'; // Kısaltılmış URL'lerin yönlendirileceği base URL
    
    // DOM Elements
    const shortenForm = document.getElementById('shorten-form');
    const originalUrlInput = document.getElementById('original-url');
    const customAliasInput = document.getElementById('custom-alias');
    const resultSection = document.getElementById('result-section');
    const shortUrlDisplay = document.getElementById('short-url');
    const originalUrlDisplay = document.getElementById('original-url-display');
    const copyBtn = document.getElementById('copy-btn');
    const urlsList = document.getElementById('urls-list');
    const toast = document.getElementById('message-toast');
    const toastMessage = document.getElementById('toast-message');
    
    // Load all URLs on page load
    loadAllUrls();
    
    // Event Listeners
    shortenForm.addEventListener('submit', handleFormSubmit);
    copyBtn.addEventListener('click', copyToClipboard);
    
    // Functions
    async function handleFormSubmit(e) {
        e.preventDefault();
        
        const originalUrl = originalUrlInput.value.trim();
        const customAlias = customAliasInput.value.trim();
        
        if (!originalUrl) {
            showToast('Please enter a valid URL', 'error');
            return;
        }
        
        try {
            const data = {
                originalUrl: originalUrl
            };
            
            if (customAlias) {
                data.customAlias = customAlias;
            }
            
            const response = await fetch(`${API_BASE_URL}/url/create`, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify(data)
            });
            
            if (!response.ok) {
                throw new Error('Failed to shorten URL');
            }
            
            const result = await response.json();
            
            // Display the result
            showResult(result);
            
            // Clear form
            shortenForm.reset();
            
            // Reload URLs list
            loadAllUrls();
            
            showToast('URL shortened successfully!', 'success');
        } catch (error) {
            console.error('Error:', error);
            showToast('Failed to shorten URL. Please try again.', 'error');
        }
    }
    
    function showResult(result) {
        // Backend URL + kısa kod şeklinde URL oluştur
        const shortUrl = `${REDIRECT_BASE_URL}/${result.shortUrl}`;
        
        // Kısaltılmış URL'yi linkli olarak göster
        shortUrlDisplay.textContent = shortUrl;
        shortUrlDisplay.href = shortUrl;
        
        // Orijinal URL'yi göster
        originalUrlDisplay.textContent = result.longUrl || result.LongUrl;
        
        // Sonuç bölümünü görünür yap
        resultSection.style.display = 'block';
    }
    
    async function copyToClipboard() {
        try {
            const text = shortUrlDisplay.textContent;
            await navigator.clipboard.writeText(text);
            showToast('Copied to clipboard!', 'success');
        } catch (err) {
            showToast('Failed to copy text', 'error');
        }
    }
    
    async function loadAllUrls() {
        try {
            const response = await fetch(`${API_BASE_URL}/url/all`);
            
            if (!response.ok) {
                if (response.status === 204) {
                    // No content
                    urlsList.innerHTML = '<tr><td colspan="3" style="text-align: center;">No URLs found</td></tr>';
                    return;
                }
                throw new Error('Failed to load URLs');
            }
            
            const urls = await response.json();
            console.log("API Response:", urls); // API cevabını kontrol etmek için
            displayUrls(urls);
        } catch (error) {
            console.error('Error loading URLs:', error);
            urlsList.innerHTML = '<tr><td colspan="3" style="text-align: center;">Failed to load URLs</td></tr>';
        }
    }

    function displayUrls(urls) {
        urlsList.innerHTML = '';
        
        if (!urls || urls.length === 0) {
            urlsList.innerHTML = '<tr><td colspan="3" style="text-align: center;">No URLs found</td></tr>';
            return;
        }
        
        // URL tablonun başlıklarını ekleyelim
        const headerRow = document.createElement('tr');
        headerRow.innerHTML = `
            <th>Original URL</th>
            <th>Short URL</th>
            <th>Actions</th>
        `;
        urlsList.appendChild(headerRow);
        
        urls.forEach(url => {
            // API'den gelen property isimlerini kontrol edelim
            const longUrl = url.longUrl || url.LongUrl; 
            const shortUrlCode = url.shortUrl || url.ShortUrl;
            const id = url.id || url.Id;
            
            const shortUrlFull = `${REDIRECT_BASE_URL}/${shortUrlCode}`;
            
            const row = document.createElement('tr');
            row.innerHTML = `
                <td class="long-url-cell">
                    <a href="${longUrl}" target="_blank" title="Visit original URL">
                        ${truncateText(longUrl, 30)}
                    </a>
                </td>
                <td class="short-url-cell">
                    <a href="${shortUrlFull}" target="_blank" title="Visit shortened URL">
                        ${shortUrlFull}
                    </a>
                </td>
                <td class="actions-cell">
                    <button class="action-btn copy-btn" data-url="${shortUrlFull}" title="Copy to clipboard">
                        <i class="fas fa-copy"></i>
                    </button>
                    <button class="action-btn delete-btn" data-id="${id}" title="Delete URL">
                        <i class="fas fa-trash"></i>
                    </button>
                </td>
            `;
            
            urlsList.appendChild(row);
            
            // Copy button event listener
            row.querySelector('.copy-btn').addEventListener('click', function() {
                navigator.clipboard.writeText(this.dataset.url)
                    .then(() => showToast('Copied to clipboard!', 'success'))
                    .catch(() => showToast('Failed to copy', 'error'));
            });
            
            // Delete button event listener
            row.querySelector('.delete-btn').addEventListener('click', function() {
                if (confirm('Are you sure you want to delete this URL?')) {
                    deleteUrl(this.dataset.id);
                }
            });
        });
    }
    
    async function deleteUrl(id) {
        try {
            const response = await fetch(`${API_BASE_URL}/url/delete/${id}`, {
                method: 'DELETE'
            });
            
            if (!response.ok) {
                throw new Error('Failed to delete URL');
            }
            
            showToast('URL deleted successfully', 'success');
            loadAllUrls();
        } catch (error) {
            console.error('Error deleting URL:', error);
            showToast('Failed to delete URL', 'error');
        }
    }
    
    function truncateText(text, maxLength) {
        if (text.length <= maxLength) return text;
        return text.substr(0, maxLength - 3) + '...';
    }
    
    function showToast(message, type = 'info') {
        toastMessage.textContent = message;
        
        // Add class based on message type
        toast.className = 'toast';
        if (type === 'error') {
            toast.style.backgroundColor = 'var(--error-color)';
        } else if (type === 'success') {
            toast.style.backgroundColor = 'var(--success-color)';
        } else {
            toast.style.backgroundColor = '#333';
        }
        
        // Show the toast
        toast.classList.add('show');
        
        // Hide after 3 seconds
        setTimeout(function() { 
            toast.classList.remove('show');
        }, 3000);
    }
});